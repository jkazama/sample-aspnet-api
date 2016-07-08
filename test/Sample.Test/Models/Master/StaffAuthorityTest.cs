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
            Assert.Equal(StaffAuthority.Find(rep, "staffA").Count, 3);
            Assert.Equal(StaffAuthority.Find(rep, "staffB").Count, 2);
        }
    }
}