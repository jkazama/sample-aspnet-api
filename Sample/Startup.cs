using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sample.Context.Rest;
using Sample.Models;

namespace Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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

            // // Identity [認証 / 認可]
            // services
            //     .AddIdentity<ApplicationUser, IdentityRole>(options =>
            //     {
            //         options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
            //         {
            //             OnRedirectToLogin = ctx =>
            //             {
            //                 ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //                 return Task.FromResult<object>(null);
            //             }
            //         };
            //     })
            //     .AddEntityFrameworkStores<Repository>()
            //     .AddDefaultTokenProviders();

            // MVC [API]
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddDataAnnotationsLocalization()
                .AddMvcOptions(options =>
                {
                    options.Filters.Add(new RestExceptionFilter());
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app
                    .UseDeveloperExceptionPage()
                    .UseDatabaseErrorPage()
                    .UseCors("AllowAll");
                DataFixtures.Initialize(app.ApplicationServices);
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}