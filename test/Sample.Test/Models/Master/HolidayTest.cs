using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sample.Models.Master
{
    public class HolidayTest : EntityTestSupport
    {
        public HolidayTest()
        {
            base.Initialize();
            new List<string> { "2015-09-21", "2015-09-22", "2015-09-23", "2016-09-21" }.ForEach(day =>
                  DataFixtures.Holiday(day).Save(rep));
        }

        [Fact]
        public void Get()
        {
            var day = Holiday.Get(rep, DateTime.Parse("2015-09-22"));
            Assert.NotNull(day);
            Assert.Equal(day.Day, DateTime.Parse("2015-09-22"));
        }

        [Fact]
        public void Find()
        {
            Assert.Equal(Holiday.Find(rep, 2015).Count, 3);
            Assert.Equal(Holiday.Find(rep, 2016).Count, 1);
        }

        [Fact]
        public void Register()
        {
            var items = new List<string> { "2016-09-21", "2016-09-22", "2016-09-23" }
                .Select(day => new RegHolidayItem { Day = DateTime.Parse(day), Name = "休日" });
            Holiday.Register(rep, new RegHoliday { Year = 2016, List = items.ToList() });
            Assert.Equal(Holiday.Find(rep, 2016).Count, 3);
        }
    }
}