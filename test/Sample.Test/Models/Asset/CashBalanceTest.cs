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
        public void PassingTest()
        {
            var cb = CashBalance.GetOrNew(rep, "test1", "JPY");
            var cb2 = CashBalance.GetOrNew(rep, "test2", "JPY");
            Assert.True(true);
        }
    }
}