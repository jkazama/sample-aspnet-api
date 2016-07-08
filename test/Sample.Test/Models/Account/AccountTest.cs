using Sample.Context;
using Xunit;

namespace Sample.Models.Account
{
    public class AccountTest : EntityTestSupport
    {
        public AccountTest()
        {
            base.Initialize();
            DataFixtures.Acc("normal").Save(rep);
        }

        [Fact]
        public void Register()
        {
            // 通常登録
            Assert.Null(Account.Get(rep, "new"));
            Account.Register(rep, new RegAccount
            {
                Id = "new",
                Name = "name",
                Mail = "new@example.com",
                PlainPassword = "password"
            });
            var m = Account.Load(rep, "new");
            Assert.Equal(m.Name, "name");
            Assert.Equal(m.Mail, "new@example.com");
            var l = Login.Load(rep, "new");
            Assert.Equal(l.Password, "password"); //TODO: 暗号化検証

            // 同一ID重複
            Assert.Throws<ValidationException>(() =>
                Account.Register(rep, new RegAccount
                {
                    Id = "normal",
                    Name = "name",
                    Mail = "new@example.com",
                    PlainPassword = "password"
                }));
        }

        [Fact]
        public void Change()
        {
            Account.Load(rep, "normal").Change(rep, new ChgAccount { Name = "changed", Mail = "changed@example.com" });
            var m = Account.Load(rep, "normal");
            Assert.Equal(m.Name, "changed");
            Assert.Equal(m.Mail, "changed@example.com");
        }

        [Fact]
        public void LoadValid()
        {
            // 通常取得
            var m = Account.LoadValid(rep, "normal");
            Assert.Equal(m.Name, "normal");
            Assert.Equal(m.StatusType, AccountStatusType.Normal);

            // 退会時取得
            var withdrawal = DataFixtures.Acc("withdrawal").Save(rep);
            withdrawal.StatusType = AccountStatusType.Withdrwal;
            withdrawal.Update(rep);

            Assert.Throws<ValidationException>(() => Account.LoadValid(rep, "withdrawal"));
        }
    }
}