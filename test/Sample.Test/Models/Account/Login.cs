using Sample.Context;
using Xunit;

namespace Sample.Models.Account
{
    public class LoginTest : EntityTestSupport
    {
        public LoginTest()
        {
            base.Initialize();
            DataFixtures.Login("test").Save(rep);
        }

        [Fact]
        public void ChangeLoginId()
        {
            // 正常系
            DataFixtures.Login("any").Save(rep);
            var m = Login.Load(rep, "any").Change(rep, new ChgLoginId { LoginId = "testAny" });
            Assert.Equal(m.Id, "any");
            Assert.Equal(m.LoginId, "testAny");

            // 自身に対する同名変更
            var self = Login.Load(rep, "any").Change(rep, new ChgLoginId { LoginId = "testAny" });
            Assert.Equal(self.Id, "any");
            Assert.Equal(self.LoginId, "testAny");

            // 同一ID重複
            Assert.Throws<ValidationException>(() =>
                Login.Load(rep, "any").Change(rep, new ChgLoginId { LoginId = "test" }));
        }

        [Fact]
        public void ChangePassword()
        {
            var m = Login.Load(rep, "test").Change(rep, new ChgPassword { PlainPassword = "changed" });
            Assert.Equal(m.Password, "changed"); //TODO: 暗号化
        }

        [Fact]
        public void GetByLoginId()
        {
            var m = Login.Load(rep, "test");
            m.LoginId = "changed";
            m.Update(rep);
            Assert.NotNull(Login.GetByLoginId(rep, "changed"));
            Assert.Null(Login.GetByLoginId(rep, "test"));
        }
    }
}