using System;
using Microsoft.Data.Entity;
using Sample.Context;
using Xunit;

namespace Sample.Models.Master
{
    public class HolidayTest
    {
        private Repository _rep;

        public HolidayTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseInMemoryDatabase();
            _rep = new Repository(optionsBuilder.Options, new DomainHelper());
        }

        [Fact]
        public void PassingTest()
        {
            var cb = Holiday.Get(_rep, DateTime.Now);
            Console.WriteLine(cb);
            Assert.True(true);
        }
    }
}