using Sample.Context;
using Xunit;

namespace Sample.Models.Master
{
    public class SelfFiAccountTest : EntityTestSupport
    {
        public SelfFiAccountTest()
        {
            base.Initialize();
            DataFixtures.SelfFiAcc("sample", "JPY").Save(rep);
        }

        [Fact]
        public void Load()
        {
            var m = SelfFiAccount.Load(rep, "sample", "JPY");
            Assert.Equal("sample", m.Category);
            Assert.Equal("JPY", m.Currency);
            Assert.Equal("sample-JPY", m.FiCode);
            Assert.Equal("xxxxxx", m.FiAccountId);
            Assert.Throws<ValidationException>(() => SelfFiAccount.Load(rep, "sample", "USD"));
        }
    }
}