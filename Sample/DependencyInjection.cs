using Microsoft.Extensions.DependencyInjection;
using Sample.Context;
using Sample.Models;
using Sample.Usecases;

namespace Sample
{
    public class DependencyInjection
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<Repository, Repository>();
            services.AddScoped<DomainHelper, DomainHelper>();
            services.AddScoped<AccountService, AccountService>();
            services.AddScoped<AssetService, AssetService>();
            services.AddScoped<MasterAdminService, MasterAdminService>();
            services.AddScoped<SystemAdminService, SystemAdminService>();
        }

    }
}