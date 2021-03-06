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
            Assert.Equal(20000M, cb.Add(rep, 19000M).Amount);
            // 20000 - 2000 = 18000
            Assert.Equal(18000M, cb.Add(rep, -2000M).Amount);
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
            Assert.Equal("test1", cbNormal.AccountId);
            Assert.Equal("JPY", cbNormal.Currency);
            Assert.Equal(1000M, cbNormal.Amount);

            // 基準日に存在していない残高の繰越検証
            var cbRoll = CashBalance.GetOrNew(rep, "test2", "JPY");
            Assert.Equal("test2", cbRoll.AccountId);
            Assert.Equal("JPY", cbRoll.Currency);
            Assert.Equal(3000M, cbRoll.Amount);

            // 残高を保有しない口座の生成検証
            var cbNew = CashBalance.GetOrNew(rep, "test3", "JPY");
            Assert.Equal("test3", cbNew.AccountId);
            Assert.Equal("JPY", cbNew.Currency);
            Assert.Equal(decimal.Zero, cbNew.Amount);
        }

    }
}