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
            Assert.Equal("normal", m.AccountId);
            Assert.Equal("sample", m.Category);
            Assert.Equal("JPY", m.Currency);
            Assert.Equal("sample-JPY", m.FiCode);
            Assert.Equal("FInormal", m.FiAccountId);
            Assert.Throws<ValidationException>(() => FiAccount.Load(rep, "normal", "sample", "USD"));
        }
    }
}