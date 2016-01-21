using Sample.Context;
using System;
using Xunit;

namespace Sample.Models.Asset
{
    public class CashInOutTest : EntityTestSupport
    {
        private const string Ccy = "JPY";
        private const string AccId = "test";

        public CashInOutTest()
        {
            base.Initialize();

            // 残高1000円の口座(test)を用意
            var baseDay = rep.Helper.Time.Day();
            DataFixtures.SelfFiAcc(Remarks.CashOut, Ccy).Save(rep);
            DataFixtures.Acc(AccId).Save(rep);
            DataFixtures.FiAcc(AccId, Remarks.CashOut, Ccy).Save(rep);
            DataFixtures.Cb(AccId, baseDay, Ccy, "1000").Save(rep);
        }

        [Fact]
        // 振込入出金を検索する
        public void Find()
        {
            var baseDay = rep.Helper.Time.Day();
            var basePlus1Day = baseDay.AddDays(1);
            var basePlus2Day = baseDay.AddDays(2);
            DataFixtures.Cio(AccId, "300", true, rep.Helper.Time.Tp()).Save(rep);
            //low: ちゃんとやると大変なので最低限の検証
            Assert.Equal(1, CashInOut.Find(rep, FindParam(baseDay, basePlus1Day)).Count);
            Assert.Equal(1, CashInOut.Find(rep, FindParam(baseDay, basePlus1Day, ActionStatusType.Unprocessed)).Count);
            Assert.Equal(0, CashInOut.Find(rep, FindParam(baseDay, basePlus1Day, ActionStatusType.Processed)).Count);
            Assert.Equal(0, CashInOut.Find(rep, FindParam(basePlus1Day, basePlus2Day, ActionStatusType.Unprocessed)).Count);
        }

        private FindCashInOut FindParam(DateTime fromDay, DateTime toDay, params ActionStatusType[] statusTypes)
        {
            return new FindCashInOut
            {
                Currency = Ccy,
                StatusTypes = statusTypes,
                UpdFromDay = fromDay,
                UpdToDay = toDay
            };
        }

        [Fact]
        // 振込出金依頼をする
        public void Withdraw()
        {
            var baseDay = rep.Helper.Time.Day();
            var basePlus3Day = baseDay.AddDays(3);

            // 超過の出金依頼 [例外]
            Assert.Throws<ValidationException>(() =>
                CashInOut.Withdraw(rep, new RegCashOut { AccountId = AccId, Currency = Ccy, AbsAmount = 1001m }));

            // 0円出金の出金依頼 [例外]
            Assert.Throws<ValidationException>(() =>
                CashInOut.Withdraw(rep, new RegCashOut { AccountId = AccId, Currency = Ccy, AbsAmount = 0m }));

            // 通常の出金依頼
            var normal = CashInOut.Withdraw(rep, new RegCashOut { AccountId = AccId, Currency = Ccy, AbsAmount = 300m });
            Assert.Equal(AccId, normal.AccountId);
            Assert.Equal(Ccy, normal.Currency);
            Assert.Equal(300m, normal.AbsAmount);
            Assert.Equal(true, normal.Withdrawal);
            Assert.Equal(baseDay, normal.RequestDay);
            Assert.Equal(baseDay, normal.EventDay);
            Assert.Equal(basePlus3Day, normal.ValueDay);
            Assert.Equal(Remarks.CashOut + "-" + Ccy, normal.TargetFiCode);
            Assert.Equal("FI" + AccId, normal.TargetFiAccountId);
            Assert.Equal(Remarks.CashOut + "-" + Ccy, normal.SelfFiCode);
            Assert.Equal("xxxxxx", normal.SelfFiAccountId);
            Assert.Equal(ActionStatusType.Unprocessed, normal.StatusType);
            Assert.Null(normal.CashflowId);

            // 拘束額を考慮した出金依頼 [例外]
            Assert.Throws<ValidationException>(() =>
                CashInOut.Withdraw(rep, new RegCashOut { AccountId = AccId, Currency = Ccy, AbsAmount = 701m }));
        }

        [Fact]
        // 振込出金依頼を取消する
        public void Cancel()
        {
            var now = rep.Helper.Time.Tp();

            // CF未発生の依頼を取消
            var normal = DataFixtures.Cio(AccId, "300", true, now).Save(rep);
            Assert.Equal(ActionStatusType.Cancelled, normal.Cancel(rep).StatusType);

            // 発生日を迎えた場合は取消できない [例外]
            var today = DataFixtures.Cio(AccId, "300", true, now);
            today.EventDay = now.Day;
            today.Save(rep);
            Assert.Throws<ValidationException>(() => today.Cancel(rep));
        }

        [Fact]
        // 振込出金依頼を例外状態とする
        public void Error()
        {
            var now = rep.Helper.Time.Tp();

            var normal = DataFixtures.Cio(AccId, "300", true, now).Save(rep);
            Assert.Equal(ActionStatusType.Error, normal.Error(rep).StatusType);

            // 処理済の時はエラーにできない [例外]
            var today = DataFixtures.Cio(AccId, "300", true, now);
            today.StatusType = ActionStatusType.Processed;
            today.Save(rep);
            Assert.Throws<ValidationException>(() => today.Error(rep));
        }

        [Fact]
        // 発生日を迎えた振込入出金をキャッシュフロー登録する
        public void Process()
        {
            var now = rep.Helper.Time.Tp();

            // 発生日未到来の処理 [例外]
            var future = DataFixtures.Cio(AccId, "300", true, now).Save(rep);
            Assert.Throws<ValidationException>(() => future.Process(rep));

            // 0円出金の出金依頼 [例外]
            Assert.Throws<ValidationException>(() =>
                CashInOut.Withdraw(rep, new RegCashOut { AccountId = AccId, Currency = Ccy, AbsAmount = 0m }));

            // 発生日到来処理
            var normal = DataFixtures.Cio(AccId, "300", true, now);
            normal.EventDay = now.Day;
            normal.Save(rep).Process(rep);
            Assert.Equal(ActionStatusType.Processed, normal.StatusType);
            Assert.NotNull(normal.CashflowId);

            // 発生させたキャッシュフローの検証
            var cf = Cashflow.Load(rep, normal.CashflowId.Value);
            Assert.Equal(AccId, cf.AccountId);
            Assert.Equal(Ccy, cf.Currency);
            Assert.Equal(-300m, cf.Amount);
            Assert.Equal(CashflowType.CashOut, cf.CashflowType);
            Assert.Equal(Remarks.CashOut, cf.Remark);
            Assert.Equal(now.Day, cf.EventDay);
            Assert.Equal(now.Day.AddDays(3), cf.ValueDay);
            Assert.Equal(ActionStatusType.Unprocessed, cf.StatusType);
        }
    }
}
