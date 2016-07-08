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
            Assert.Equal(m.Category, "sample");
            Assert.Equal(m.Currency, "JPY");
            Assert.Equal(m.FiCode, "sample-JPY");
            Assert.Equal(m.FiAccountId, "xxxxxx");
            Assert.Throws<ValidationException>(() => SelfFiAccount.Load(rep, "sample", "USD"));
        }
    }
}