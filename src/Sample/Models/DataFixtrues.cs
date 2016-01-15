using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sample.Models.Account;
using Sample.Models.Asset;
using Sample.Models.Master;

namespace Sample.Models
{
    public static class DataFixtures
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
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
            if (rep.Accounts.Count() == 0)
            {
                var idSample = "sample";
                rep.Accounts.Add(Acc(idSample));
            }
            rep.SaveChanges();
        }

        // account

        /** 口座の簡易生成 TODO: パスワード暗号化 */
        public static Account.Account Acc(string id)
        {
            return new Account.Account { Id = id, Name = id, Mail = "hoge@example.com", StatusType = AccountStatusType.Normal };
        }
        public static Login Login(string id)
        {
            return new Login { Id = id, LoginId = id, Password = id };
        }

        /** 口座に紐付く金融機関口座の簡易生成 */
        public static FiAccount FiAcc(string accountId, string category, string currency)
        {
            return new FiAccount { AccountId = accountId, Category = category, Currency = currency, FiCode = category + "-" + currency, FiAccountId = "FI" + accountId };
        }

        // asset

        /** 口座残高の簡易生成 */
        public static CashBalance Cb(string accountId, DateTime baseDay, string currency, string amount)
        {
            return new CashBalance { AccountId = accountId, BaseDay = baseDay, Currency = currency, Amount = decimal.Parse(amount), UpdateDate = DateTime.Now };
        }

        // master

        /** 社員の簡易生成 TODO: パスワード暗号化 */
        public static Staff Staff(string id)
        {
            return new Staff { Id = id, Name = id, Password = id };
        }

        /** 社員権限の簡易生成 */
        public static List<StaffAuthority> StaffAuth(string id, params string[] authorities)
        {
            return authorities.Select(auth =>
                new StaffAuthority { StaffId = id, Authority = auth }).ToList();
        }

        /** 自社金融機関口座の簡易生成 */
        public static SelfFiAccount SelfFiAcc(string category, string currency)
        {
            return new SelfFiAccount
            {
                Category = category,
                Currency = currency,
                FiCode = category + "-" + currency,
                FiAccountId = "xxxxxx"
            };
        }

        /** 祝日の簡易生成 */
        public static Holiday Holiday(string dayStr)
        {
            return new Holiday { Category = Master.Holiday.CategoryDefault, Name = "休日サンプル", Day = DateTime.Parse(dayStr) };
        }
    }
}