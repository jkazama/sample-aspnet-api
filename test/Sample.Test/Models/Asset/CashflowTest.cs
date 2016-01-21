using Sample.Context;
using Xunit;

namespace Sample.Models.Asset
{
    public class CashflowTest : EntityTestSupport
    {
        public CashflowTest()
        {
            base.Initialize();
        }

        [Fact]
        // キャッシュフローを登録する
        public void Register()
        {
            var baseDay = rep.Helper.Time.Day();
            var baseMinus1Day = baseDay.AddDays(-1);
            var basePlus1Day = baseDay.AddDays(1);
            // 過去日付の受渡でキャッシュフロー発生 [例外]
            Assert.Throws<ValidationException>(() =>
                Cashflow.Register(rep, DataFixtures.CfReg("test1", "1000", baseMinus1Day)));
            // 翌日受渡でキャッシュフロー発生
            var m = Cashflow.Register(rep, DataFixtures.CfReg("test1", "1000", basePlus1Day));
            Assert.Equal(1000m, m.Amount);
            Assert.Equal(ActionStatusType.Unprocessed, m.StatusType);
            Assert.Equal(baseDay, m.EventDay);
            Assert.Equal(basePlus1Day, m.ValueDay);
        }

        [Fact]
        // 未実現キャッシュフローを実現する
        public void Realize()
        {
            var baseDay = rep.Helper.Time.Day();
            var baseMinus1Day = baseDay.AddDays(-1);
            var baseMinus2Day = baseDay.AddDays(-2);
            var basePlus1Day = baseDay.AddDays(1);

            CashBalance.GetOrNew(rep, "test1", "JPY");

            // 未到来の受渡日 [例外]
            var cfFuture = DataFixtures.Cf("test1", "1000", baseDay, basePlus1Day).Save(rep);
            Assert.Throws<ValidationException>(() =>
                cfFuture.Realize(rep));

            // キャッシュフローの残高反映検証。  0 + 1000 = 1000
            var cfNormal = DataFixtures.Cf("test1", "1000", baseMinus1Day, baseDay).Save(rep);
            Assert.Equal(ActionStatusType.Processed, cfNormal.Realize(rep).StatusType);
            Assert.Equal(1000m, CashBalance.GetOrNew(rep, "test1", "JPY").Amount);

            // 処理済キャッシュフローの再実現 [例外]
            Assert.Throws<ValidationException>(() =>
                cfNormal.Realize(rep));
            
            // 過日キャッシュフローの残高反映検証。 1000 + 2000 = 3000
            var cfPast = DataFixtures.Cf("test1", "2000", baseMinus2Day, baseMinus1Day).Save(rep);
            Assert.Equal(ActionStatusType.Processed, cfPast.Realize(rep).StatusType);
            Assert.Equal(3000m, CashBalance.GetOrNew(rep, "test1", "JPY").Amount);
        }

        [Fact]
        // 発生即実現のキャッシュフローを登録する
        public void RegisterRealize()
        {
            var baseDay = rep.Helper.Time.Day();
            CashBalance.GetOrNew(rep, "test1", "JPY");
            // 発生即実現
            Cashflow.Register(rep, DataFixtures.CfReg("test1", "1000", baseDay));
            Assert.Equal(1000m, CashBalance.GetOrNew(rep, "test1", "JPY").Amount);
        }

    }
}
