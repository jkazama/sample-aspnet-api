using Sample.Context;
using Xunit;

namespace Sample.Models.Master
{
    public class StaffTest : EntityTestSupport
    {
        public StaffTest()
        {
            base.Initialize();
            DataFixtures.Staff("sample").Save(rep);
        }

        [Fact]
        public void Register()
        {
            var staff = Staff.Register(rep, new RegStaff { Id = "new", Name = "newName", PlainPassword = "password" });
            Assert.Equal("new", staff.Id);
            Assert.Equal("newName", staff.Name);
            Assert.Equal("password", staff.Password); //TODO: 暗号化一致検証
            // 重複ID
            Assert.Throws<ValidationException>(() =>
                Staff.Register(rep, new RegStaff { Id = "sample", Name = "newName", PlainPassword = "password" }));
        }

        [Fact]
        public void ChangePassword()
        {
            var changed = Staff.Load(rep, "sample").Change(rep, new ChgPassword { PlainPassword = "changed" });
            Assert.Equal("changed", changed.Password); //TODO: 暗号化一致検証
        }

        [Fact]
        public void ChangeStaff()
        {
            var changed = Staff.Load(rep, "sample").Change(rep, new ChgStaff { Name = "changed" });
            Assert.Equal("changed", changed.Name); //TODO: 暗号化一致検証
        }

        [Fact]
        public void Find()
        {
            Assert.True(0 < Staff.Find(rep, new FindStaff { Keyword = "amp" }).Count);
            Assert.False(0 < Staff.Find(rep, new FindStaff { Keyword = "amq" }).Count);
        }
    }
}