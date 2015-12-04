using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sample.Context;
using Sample.Context.Rest;
using Sample.Models;
using Sample.Usecases;

namespace Sample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDependencyInjection(services);
            services
                .AddEntityFramework()
                .AddSqlite()
                .AddDbContext<Repository>(options =>
                    options.UseSqlite(Configuration["Data:ConnectionString"]));
            services
                .AddMvc()
                .AddMvcOptions(options =>
                {
                    options.Filters.Add(new RestExceptionFilter());
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
        }
        private void ConfigureDependencyInjection(IServiceCollection services)
        {
            services.AddSingleton<DomainHelper, DomainHelper>();
            services.AddSingleton<AccountService, AccountService>();
            services.AddSingleton<AssetService, AssetService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseIISPlatformHandler();
            app.UseStaticFiles();
            app.UseMvc();
            DataFixtures.Initialize(app.ApplicationServices);
        }

        // Entry point for the application.
        public static void Main(string[] args) => Microsoft.AspNet.Hosting.WebApplication.Run<Startup>(args);
    }
}
