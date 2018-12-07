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
            Assert.Equal("name", m.Name);
            Assert.Equal("new@example.com", m.Mail);
            var l = Login.Load(rep, "new");
            Assert.Equal("password", l.Password); //TODO: 暗号化検証

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
            Assert.Equal("changed", m.Name);
            Assert.Equal("changed@example.com", m.Mail);
        }

        [Fact]
        public void LoadValid()
        {
            // 通常取得
            var m = Account.LoadValid(rep, "normal");
            Assert.Equal("normal", m.Name);
            Assert.Equal(AccountStatusType.Normal, m.StatusType);

            // 退会時取得
            var withdrawal = DataFixtures.Acc("withdrawal").Save(rep);
            withdrawal.StatusType = AccountStatusType.Withdrwal;
            withdrawal.Update(rep);

            Assert.Throws<ValidationException>(() => Account.LoadValid(rep, "withdrawal"));
        }
    }
}