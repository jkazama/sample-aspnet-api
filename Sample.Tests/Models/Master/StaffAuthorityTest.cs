using Xunit;

namespace Sample.Models.Master
{
    public class StaffAuthorityTest : EntityTestSupport
    {
        public StaffAuthorityTest()
        {
            base.Initialize();
            DataFixtures.StaffAuth("staffA", "ID000001", "ID000002", "ID000003").ForEach(m => m.Save(rep));
            DataFixtures.StaffAuth("staffB", "ID000001", "ID000002").ForEach(m => m.Save(rep));
        }

        [Fact]
        public void Find()
        {
            Assert.Equal(3, StaffAuthority.Find(rep, "staffA").Count);
            Assert.Equal(2, StaffAuthority.Find(rep, "staffB").Count);
        }
    }
}