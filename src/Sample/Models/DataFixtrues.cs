using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sample.Models.Account;

namespace Sample.Models
{
    public static class DataFixtures
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using(var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var rep = serviceScope.ServiceProvider.GetService<Repository>();
                if (rep.Database.EnsureCreated())
                {
                    InsertTestData(rep);
                }
            }
        }

        private static void InsertTestData(Repository rep)
        {
            if (rep.Accounts.Count() == 0) {
                var idSample = "sample";
                rep.Accounts.Add(Acc(idSample));
            }
            // Database
            // var models = (ModelContext)serviceProvider.GetService(typeof(ModelContext));
            // models.Accounts.Add(
            //     new Account.Account { Id = "sample", Name = "sample" , Mail = "sample@example.com", StatusType = AccountStatusType.Normal }
            // );
            // models.SaveChanges();
        }

        // 口座の簡易生成
        public static Account.Account Acc(string id) {
            return new Account.Account { Id = id, Name = id, Mail = "hoge@example.com", StatusType = AccountStatusType.Normal };
        }
    }
}