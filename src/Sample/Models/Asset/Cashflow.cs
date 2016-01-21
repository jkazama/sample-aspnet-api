using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Sample.Context;
using Sample.Context.Orm;
using Sample.Utils;
using System.Collections.Generic;

namespace Sample.Models.Asset
{
    //<summary>
    // 入出金キャッシュフローを表現します。
    // キャッシュフローは振込/振替といったキャッシュフローアクションから生成される確定状態(依頼取消等の無い)の入出金情報です。
    // low: 概念を伝えるだけなので必要最低限の項目で表現しています。
    // low: 検索関連は主に経理確認や帳票等での利用を想定します
    //</summary>
    public class Cashflow : OrmActiveRecord<Cashflow>
    {
        /** ID */
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        /** 口座ID */
        public string AccountId { get; set; }
        /** 通貨 */
        public string Currency { get; set; }
        /** 金額 */
        public decimal Amount { get; set; }
        /** 入出金 */
        public CashflowType CashflowType { get; set; }
        /** 適用 */
        public string Remark { get; set; }
        /** 発生日/日時 */
        public DateTime EventDay { get; set; }
        public DateTime EventDate { get; set; }
        /** 受渡日 */
        public DateTime ValueDay { get; set; }
        /** 処理種別 */
        public ActionStatusType StatusType { get; set; }

        //<summary>キャッシュフローを処理済みにして残高へ反映します。</summary>
        public Cashflow Realize(Repository rep)
        {
            Validate(v =>
            {
                v.Verify(CanRealize(rep), AssetErrorKeys.CashflowRealizeDay);
                v.Verify(StatusType.IsUnprocessing(), ErrorKeys.ActionUnprocessing);
            });

            StatusType = ActionStatusType.Processed;
            Update(rep);
            CashBalance.GetOrNew(rep, AccountId, Currency).Add(rep, Amount);
            return this;
        }

        //<summary>キャッシュフローを実現(受渡)可能か判定します。</summary>
        public Cashflow Error(Repository rep)
        {
            Validate(v => v.Verify(StatusType.IsUnprocessed(), ErrorKeys.ActionUnprocessing));

            StatusType = ActionStatusType.Error;
            return Update(rep);
        }

        //<summary>キャッシュフローを実現(受渡)可能か判定します。</summary>
        public bool CanRealize(Repository rep)
        {
            return rep.Helper.Time.Tp().AfterEqualsDay(ValueDay);
        }

        //<summary>キャッシュフローを取得します。（例外付）</summary>
        public static Cashflow Load(Repository rep, long id)
        {
            return rep.Load<Cashflow>(m => m.Id == id);
        }

        //<summary>指定受渡日時点で未実現のキャッシュフロー一覧を検索します。(口座通貨別)</summary>
        public static List<Cashflow> FindUnrealize(Repository rep, string accountId, string currency, DateTime valueDay)
        {
            return rep.Template<Cashflow>().Find(
                c => c.AccountId == accountId && c.Currency == currency && c.ValueDay <= valueDay
                        && ActionStatusTypes.UnprocessingTypes.Contains(c.StatusType),
                query => query.OrderBy(c => c.Id));
        }

        //<summary>指定受渡日で実現対象となるキャッシュフロー一覧を検索します。</summary>
        public static List<Cashflow> FindDoRealize(Repository rep, DateTime valueDay)
        {
            return rep.Template<Cashflow>().Find(
                c => c.ValueDay <= valueDay && ActionStatusTypes.UnprocessedTypes.Contains(c.StatusType),
                query => query.OrderBy(c => c.Id));
        }

        //<summary>
        // キャッシュフローを登録します。
        // 受渡日を迎えていた時はそのまま残高へ反映します。
        //</summary>
        public static Cashflow Register(Repository rep, RegCashflow p)
        {
            var now = rep.Helper.Time.Tp();
            Validator.Validate(v => v.CheckField(now.BeforeEqualsDay(p.ValueDay), "valueDay", AssetErrorKeys.CashflowBeforeEqualsDay));
            var cf = p.Create(now).Save(rep);
            return cf.CanRealize(rep) ? cf.Realize(rep) : cf;
        }

    }

    //<summary>キャッシュフロー種別。 low: 各社固有です。摘要含めラベルはなるべく外部リソースへ切り出し</summary>
    public enum CashflowType
    {
        /** 振込入金 */
        CashIn,
        /** 振込出金 */
        CashOut,
        /** 振替入金 */
        CashTransferIn,
        /** 振替出金 */
        CashTransferOut
    }

    //<summary>入出金キャッシュフローの登録パラメタ</summary>
    public class RegCashflow : IDto
    {
        public string AccountId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public CashflowType CashflowType { get; set; }
        public string Remark { get; set; }
        public DateTime? EventDay { get; set; }
        public DateTime ValueDay { get; set; }

        public Cashflow Create(TimePoint now)
        {
            var eventDate = EventDay.HasValue ? new TimePoint { Day = EventDay.Value, Date = now.Date } : now;
            return new Cashflow
            {
                AccountId = AccountId,
                Currency = Currency,
                Amount = Amount,
                CashflowType = CashflowType,
                Remark = Remark,
                EventDay = eventDate.Day,
                EventDate = eventDate.Date,
                ValueDay = ValueDay,
                StatusType = ActionStatusType.Unprocessed
            };
        }
    }

}