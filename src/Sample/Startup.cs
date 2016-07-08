using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sample.Context.Rest;
using Sample.Models;
using Sample.Models.Account;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Sample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        //<summary>
        // サービスを登録します。
        //</summary>
        public void ConfigureServices(IServiceCollection services)
        {
            // Local Dependency Injection
            DependencyInjection.Configure(services);

            // i18n [国際化]
            services
                .AddLocalization(options => options.ResourcesPath = "Resources");

            // Entity Framework [データアクセス]
            services
                .AddEntityFrameworkSqlite()
                .AddDbContext<Repository>(options =>
                    options.UseSqlite(Configuration["Data:ConnectionString"]));
            
            // Identity [認証 / 認可]
            services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                    {
                        OnRedirectToLogin = ctx =>
                        {
                            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            return Task.FromResult<object>(null);
                        }
                    };
                })
                .AddEntityFrameworkStores<Repository>()
                .AddDefaultTokenProviders();

            // MVC [API]
            services
                .AddMvc()
                .AddDataAnnotationsLocalization()
                .AddMvcOptions(options =>
                {
                    options.Filters.Add(new RestExceptionFilter());
                    //options.Filters.Add(new DummyLoginFilter());
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services
                .AddCors(options =>
                    options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials()));
        }

        //<summary>
        // サービスを有効化します。
        //</summary>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("ja") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ja"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseCors("AllowAll");
                DataFixtures.Initialize(app.ApplicationServices);
            }
            if (Boolean.Parse(Configuration["Security:Enabled"]))
            {
                app.UseIdentity();
            }
            app.UseMvc();
        }
    }
}
