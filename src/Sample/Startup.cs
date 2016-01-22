using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sample.Context.Rest;
using Sample.Models;

namespace Sample
{
    public class Startup
    {
        private IHostingEnvironment _env;
        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            DependencyInjections.Configure(services);
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
            if (_env.IsDevelopment())
            {
                services
                    .AddCors(options =>
                        options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials()));
            }
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseIISPlatformHandler();
            app.UseStaticFiles();
            if (_env.IsDevelopment())
            {
                app.UseCors("AllowAll");
                DataFixtures.Initialize(app.ApplicationServices);
            }
            app.UseMvc();
        }

        // Entry point for the application.
        public static void Main(string[] args) => Microsoft.AspNet.Hosting.WebApplication.Run<Startup>(args);
    }
}
