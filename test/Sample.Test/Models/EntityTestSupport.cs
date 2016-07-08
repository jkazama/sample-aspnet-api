using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sample.Context;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Sample.Models
{
    public abstract class EntityTestSupport
    {
        protected Repository rep { get; set; }

        protected virtual void Initialize(LogLevel logLevel = LogLevel.Warning)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            var ds = "Filename=./test-" + this.GetType().FullName + ".db";
            optionsBuilder.UseSqlite(ds);
            this.rep = new Repository(optionsBuilder.Options, new DomainHelper());
            // for log
            var contextServices = ((IInfrastructure<IServiceProvider>)rep).Instance;
            var loggerFactory = contextServices.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddConsole(logLevel);
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