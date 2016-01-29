using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Localization;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sample.Context;
using Sample.Context.Rest;
using Sample.Models;
using Sample.Models.Account;
using System;
using System.Net;
using System.Threading.Tasks;

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

        //<summary>
        // サービス登録をします。
        //</summary>
        public void ConfigureServices(IServiceCollection services)
        {
            DependencyInjection.Configure(services);
            
            // i18n [国際化]
            services
                .AddLocalization(options => options.ResourcesPath = "Resources");

            // Entity Framework [データアクセス]
            services
                .AddEntityFramework()
                .AddSqlite()
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IStringLocalizer<Startup> SR)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseRequestLocalization(new RequestCulture("en"));
            app.UseIISPlatformHandler();
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
        
        // 起動処理
        public static void Main(string[] args) => Microsoft.AspNet.Hosting.WebApplication.Run<Startup>(args);
    }

    //public class DummyLoginFilter : IAuthorizationFilter
    //{
    //    public async void OnAuthorization(AuthorizationContext context)
    //    {
    //        var hoge = "";
    //        var service = context.HttpContext.ApplicationServices.GetService<SignInManager<ApplicationUser>>();
            
    //        var ret = await service.PasswordSignInAsync(new ApplicationUser { Id = "hoge" }, "", false, false);
    //        await service.SignInAsync(new ApplicationUser { Id = "sample" }, true);
    //        //context.HttpContext.Authentication
    //    }
    //}

}
