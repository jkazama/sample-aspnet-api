using Xunit;

namespace Sample.Models.Asset
{
    public class CashBalanceTest : EntityTestSupport
    {
        public CashBalanceTest()
        {
            base.Initialize();
        }

        [Fact]
        public void Add()
        {
            var baseDay = rep.Helper.Time.Day();
            var cb = DataFixtures.Cb("test1", baseDay, "JPY", "1000").Save(rep);
            // 1000 + 19000 = 20000
            Assert.True(cb.Add(rep, 19000M).Amount == 20000M);
            // 20000 - 2000 = 18000
            Assert.True(cb.Add(rep, -2000M).Amount == 18000M);
        }

        [Fact]
        public void GetOrNew()
        {
            var baseDay = rep.Helper.Time.Day();
            var baseMinus1Day = baseDay.AddDays(-1);

            DataFixtures.Cb("test1", baseDay, "JPY", "1000").Save(rep);
            DataFixtures.Cb("test2", baseMinus1Day, "JPY", "3000").Save(rep);

            // 存在している残高の検証
            var cbNormal = CashBalance.GetOrNew(rep, "test1", "JPY");
            Assert.True(cbNormal.AccountId == "test1");
            Assert.True(cbNormal.Currency == "JPY");
            Assert.True(cbNormal.Amount == 1000M);

            // 基準日に存在していない残高の繰越検証
            var cbRoll = CashBalance.GetOrNew(rep, "test2", "JPY");
            Assert.True(cbRoll.AccountId == "test2");
            Assert.True(cbRoll.Currency == "JPY");
            Assert.True(cbRoll.Amount == 3000M);

            // 残高を保有しない口座の生成検証
            var cbNew = CashBalance.GetOrNew(rep, "test3", "JPY");
            Assert.True(cbNew.AccountId == "test3");
            Assert.True(cbNew.Currency == "JPY");
            Assert.True(cbNew.Amount == decimal.Zero);
        }

    }
}