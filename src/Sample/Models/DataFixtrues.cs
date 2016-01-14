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
                //TODO: 開発環境のみに制約
                DataFixtures.Initialize(serviceScope.ServiceProvider.GetService<Repository>());
            }
        }
        public static void Initialize(Repository rep)
        {
            if (rep.Database.EnsureDeleted() && rep.Database.EnsureCreated())
            {
                InsertTestData(rep);
            }
        }

        private static void InsertTestData(Repository rep)
        {
            if (rep.Accounts.Count() == 0) {
                var idSample = "sample";
                rep.Accounts.Add(Acc(idSample));
            }
            rep.SaveChanges();
        }

        // 口座の簡易生成
        public static Account.Account Acc(string id) {
            return new Account.Account { Id = id, Name = id, Mail = "hoge@example.com", StatusType = AccountStatusType.Normal };
        }
    }
}