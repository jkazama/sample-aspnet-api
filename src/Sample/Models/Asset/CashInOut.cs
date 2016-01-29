using Sample.Context;
using Sample.Context.Orm;
using Sample.Models.Account;
using Sample.Models.Constraints;
using Sample.Models.Master;
using Sample.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Sample.Models.Asset
{
    //<summary>
    // 振込入出金依頼を表現するキャッシュフローアクション。
    // <p>相手方/自社方の金融機関情報は依頼後に変更される可能性があるため、依頼時点の状態を保持するために非正規化しています。
    // low: 相手方/自社方の金融機関情報は項目数が多いのでサンプル用に金融機関コードのみにしています。
    // 実際の開発ではそれぞれ複合クラス(FinantialInstitution)に束ねるアプローチを推奨します。
    //</summary>
    public class CashInOut : OrmActiveRecord<CashInOut>
    {
        /** ID */
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        /** 口座ID */
        public string AccountId { get; set; }
        /** 通貨 */
        public string Currency { get; set; }
        /** 金額(絶対値) */
        public decimal AbsAmount { get; set; }
        /** 出金時はtrue */
        public bool Withdrawal { get; set; }
        /** 依頼日/日時 */
        public DateTime RequestDay { get; set; }
        public DateTime RequestDate { get; set; }
        /** 発生日 */
        public DateTime EventDay { get; set; }
        /** 受渡日 */
        public DateTime ValueDay { get; set; }
        /** 相手方金融機関コード */
        public string TargetFiCode { get; set; }
        /** 相手方金融機関口座ID */
        public string TargetFiAccountId { get; set; }
        /** 自社方金融機関コード */
        public string SelfFiCode { get; set; }
        /** 自社方金融機関口座ID */
        public string SelfFiAccountId { get; set; }
        /** 処理種別 */
        public ActionStatusType StatusType { get; set; }
        /** キャッシュフローID。処理済のケースでのみ設定されます。low: 実際は調整CFや消込CFの概念なども有 */
        public long? CashflowId { get; set; }
        /** 更新日時 */
        public DateTime UpdateDate { get; set; }

        //<summary>
        // 依頼を処理します。
        // <p>依頼情報を処理済にしてキャッシュフローを生成します。
        //</summary>
        public CashInOut Process(Repository rep)
        {
            //low: 出金営業日の取得。ここでは単純な営業日を取得
            var now = rep.Helper.Time.Tp();
            // 事前審査
            Validate(v =>
            {
                v.Verify(StatusType.IsUnprocessed(), ErrorKeys.ActionUnprocessing);
                v.Verify(now.AfterEqualsDay(EventDay), AssetErrorKeys.CashInOutAfterEqualsDay);
            });
            // 処理済状態を反映
            StatusType = ActionStatusType.Processed;
            CashflowId = Cashflow.Register(rep, RegCf()).Id;
            UpdateDate = now.Date;
            return Update(rep);
        }

        private RegCashflow RegCf()
        {
            var amount = Withdrawal ? -AbsAmount : AbsAmount;
            var cashflowType = Withdrawal ? CashflowType.CashOut : CashflowType.CashIn;
            //low: 摘要はとりあえずシンプルに。実際はCashInOutへ用途フィールドをもたせた方が良い(生成元メソッドに応じて摘要を変える)
            var remark = Withdrawal ? Remarks.CashOut : Remarks.CashInOut;
            return new RegCashflow {
                AccountId = AccountId,
                Currency = Currency,
                Amount = amount,
                CashflowType = cashflowType,
                Remark = remark,
                EventDay = EventDay,
                ValueDay = ValueDay
            };
        }

        //<summary>
        // 依頼を取消します。
        // <p>依頼情報を処理済にしてキャッシュフローを生成します。
        //</summary>
        public CashInOut Cancel(Repository rep)
        {
            var now = rep.Helper.Time.Tp();
            // 事前審査
            Validate(v =>
            {
                v.Verify(StatusType.IsUnprocessing(), ErrorKeys.ActionUnprocessing);
                v.Verify(now.BeforeDay(EventDay), AssetErrorKeys.CashInOutBeforeEqualsDay);
            });
            // 取消状態を反映
            StatusType = ActionStatusType.Cancelled;
            UpdateDate = now.Date;
            return Update(rep);
        }

        //<summary>
        // 依頼をエラー状態にします。
        // <p>処理中に失敗した際に呼び出してください。
        // low: 実際はエラー事由などを引数に取って保持する
        //</summary>
        public CashInOut Error(Repository rep)
        {
            // 事前審査
            Validate(v => v.Verify(StatusType.IsUnprocessed(), ErrorKeys.ActionUnprocessing));

            // 取消状態を反映
            StatusType = ActionStatusType.Error;
            UpdateDate = rep.Helper.Time.Date();
            return Update(rep);
        }

        //<summary>振込入出金依頼を返します。</summary>
        public static CashInOut Load(Repository rep, long id)
        {
            return rep.Load<CashInOut>(m => m.Id == id);
        }

        //<summary>未処理の振込入出金依頼一覧を検索します。 low: 可変条件の設定例</summary>
        public static List<CashInOut> Find(Repository rep, FindCashInOut p)
        {
            // low: 通常であれば事前にfrom/toの期間チェックを入れる
            var criteria = rep.Criteria<CashInOut>();
            var toDay = DateUtils.DateTo(p.UpdToDay);
            criteria.Add(c => p.UpdFromDay.Date <= c.UpdateDate && c.UpdateDate <= toDay);
            criteria.Add(p.Currency != null,
                c => c.Currency == p.Currency);
            criteria.Add(0 < p.StatusTypes.Length,
                c => p.StatusTypes.Contains(c.StatusType));
            return rep.Template<CashInOut>().Find(criteria.Predicates(),
                query => query.OrderByDescending(c => c.UpdateDate));
        }

        //<summary>当日発生で未処理の振込入出金一覧を検索します。</summary>
        public static List<CashInOut> FindUnprocessed(Repository rep)
        {
            return rep.Template<CashInOut>().Find(
                c => c.EventDay == rep.Helper.Time.Day()
                    && ActionStatusTypes.UnprocessedTypes.Contains(c.StatusType),
                query => query.OrderBy(c => c.Id));
        }

        //<summary>未処理の振込入出金一覧を検索します。(口座別)</summary>
        public static List<CashInOut> FindUnprocessed(Repository rep, string accountId, string currency, bool withdrawal)
        {
            return rep.Template<CashInOut>().Find(
                c => c.AccountId == accountId && c.Currency == currency && c.Withdrawal == withdrawal
                    && ActionStatusTypes.UnprocessedTypes.Contains(c.StatusType),
                query => query.OrderBy(c => c.Id));
        }

        //<summary>未処理の振込入出金一覧を検索します。(口座別)</summary>
        public static List<CashInOut> FindUnprocessed(Repository rep, string accountId)
        {
            return rep.Template<CashInOut>().Find(
                c => c.AccountId == accountId
                    && ActionStatusTypes.UnprocessedTypes.Contains(c.StatusType),
                query => query.OrderByDescending(c => c.UpdateDate));
        }

        //<summary>出金依頼をします。</summary>
        public static CashInOut Withdraw(Repository rep, RegCashOut p)
        {
            var now = rep.Helper.Time.Tp();
            // low: 発生日は締め時刻等の兼ね合いで営業日と異なるケースが多いため、別途DB管理される事が多い
            var eventDay = now.Day;
            // low: 実際は各金融機関/通貨の休日を考慮しての T+N 算出が必要
            var valueDay = eventDay.AddDays(3);

            // 事前審査
            Utils.Validator.Validate(v =>
            {
                v.VerifyField(0 < p.AbsAmount, "absAmount", DomainErrorKeys.AbsAmountZero);
                v.VerifyField(
                    Asset.Of(p.AccountId).CanWithdraw(rep, p.Currency, p.AbsAmount, valueDay),
                    "absAmount", AssetErrorKeys.CashInOutWithdrawAmount);
            });

            // 出金依頼情報を登録
            var acc = FiAccount.Load(rep, p.AccountId, Remarks.CashOut, p.Currency);
            var selfAcc = SelfFiAccount.Load(rep, Remarks.CashOut, p.Currency);
            return p.Create(now, eventDay, valueDay, acc, selfAcc).Save(rep);
        }
    }

    //<summary>振込入出金依頼の検索パラメタ。 low: 通常は顧客視点/社内視点で利用条件が異なる</summary>
    public class FindCashInOut : IDto
    {
        public string Currency { get; set; }
        public ActionStatusType[] StatusTypes { get; set; }
        public DateTime UpdFromDay { get; set; }
        public DateTime UpdToDay { get; set; }
    }

    //<summary>振込出金の依頼パラメタ。</summary>
    public class RegCashOut : IDto
    {
        [Required]
        public string AccountId { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required, AbsAmount]
        public decimal AbsAmount { get; set; }

        public CashInOut Create(TimePoint now, DateTime eventDay, DateTime valueDay, FiAccount acc, SelfFiAccount selfAcc)
        {
            return new CashInOut
            {
                AccountId = AccountId,
                Currency = Currency,
                AbsAmount = AbsAmount,
                Withdrawal = true,
                RequestDay = now.Day,
                RequestDate = now.Date,
                EventDay = eventDay,
                ValueDay = valueDay,
                TargetFiCode = acc.FiCode,
                TargetFiAccountId = acc.FiAccountId,
                SelfFiCode = selfAcc.FiCode,
                SelfFiAccountId = selfAcc.FiAccountId,
                StatusType = ActionStatusType.Unprocessed,
                UpdateDate = now.Date
            };
        }
    }

}
