using Microsoft.Extensions.DependencyInjection;
using Sample.Context;
using Sample.Usecases;

namespace Sample
{
    public class DependencyInjection
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddSingleton<DomainHelper, DomainHelper>();
            services.AddSingleton<AccountService, AccountService>();
            services.AddSingleton<AssetService, AssetService>();
            services.AddSingleton<MasterAdminService, MasterAdminService>();
            services.AddSingleton<SystemAdminService, SystemAdminService>();
        }

    }
}
