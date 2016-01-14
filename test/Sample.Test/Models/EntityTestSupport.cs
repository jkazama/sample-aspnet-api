using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sample.Context;

namespace Sample.Models
{
    public abstract class EntityTestSupport
    {
        protected Repository rep { get; set; }

        protected virtual void Initialize()
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlite("Data Source=./test.db");
            this.rep = new Repository(optionsBuilder.Options, new DomainHelper());
            // for log
            var contextServices = ((IInfrastructure<IServiceProvider>)rep).Instance;
            var loggerFactory = contextServices.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddConsole(LogLevel.Information);
            // db drop / create
            DataFixtures.Initialize(rep);
        }

        protected void Dump(object v)
        {
            Console.WriteLine("=========================");
            Console.WriteLine(v);
            Console.WriteLine("=========================");
        }
    }
}