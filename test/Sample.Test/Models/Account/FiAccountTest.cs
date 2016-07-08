using Sample.Context;
using Xunit;

namespace Sample.Models.Account
{
    public class FiAccountTest : EntityTestSupport
    {
        public FiAccountTest()
        {
            base.Initialize();
            DataFixtures.FiAcc("normal", "sample", "JPY").Save(rep);
        }

        [Fact]
        public void Load()
        {
            var m = FiAccount.Load(rep, "normal", "sample", "JPY");
            Assert.Equal(m.AccountId, "normal");
            Assert.Equal(m.Category, "sample");
            Assert.Equal(m.Currency, "JPY");
            Assert.Equal(m.FiCode, "sample-JPY");
            Assert.Equal(m.FiAccountId, "FInormal");
            Assert.Throws<ValidationException>(() => FiAccount.Load(rep, "normal", "sample", "USD"));
        }
    }
}