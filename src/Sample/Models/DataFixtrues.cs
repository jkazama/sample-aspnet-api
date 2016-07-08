using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sample.Models.Account;
using Sample.Models.Asset;
using Sample.Models.Master;
using Sample.Utils;
using Sample.Context;

namespace Sample.Models
{
    //<summary>
    // データ生成用のサポートコンポーネント。
    // <p>テストや開発時の簡易マスタデータ生成を目的としているため本番での利用は想定していません。
    //</summary>
    public static class DataFixtures
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
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
                var ccy = "JPY";
                var baseDay = TimePoint.Now().Day;

                // 社員: admin (passも同様)
                Staff("admin").Save(rep);

                // 自社金融機関
                SelfFiAcc(Remarks.CashOut, ccy).Save(rep);

                // 口座: sample (passも同様)
                var idSample = "sample";
                Acc(idSample).Save(rep);
                Login(idSample).Save(rep);
                FiAcc(idSample, Remarks.CashOut, ccy).Save(rep);
                Cb(idSample, baseDay, ccy, "1000000").Save(rep);
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

        /** キャッシュフローの簡易生成 */
        public static Cashflow Cf(String accountId, String amount, DateTime eventDay, DateTime valueDay)
        {
            return CfReg(accountId, amount, valueDay).Create(TimePoint.Of(eventDay));
        }

        /** キャッシュフロー登録パラメタの簡易生成 */
        public static RegCashflow CfReg(string accountId, string amount, DateTime valueDay)
        {
            return new RegCashflow {
                AccountId = accountId,
                Currency = "JPY",
                Amount = decimal.Parse(amount),
                CashflowType = CashflowType.CashIn,
                Remark = "cashIn",
                EventDay = null,
                ValueDay = valueDay
            };
        }

        /** 振込入出金依頼の簡易生成 [発生日(T+1)/受渡日(T+3)] */
        public static CashInOut Cio(string accountId, string absAmount, bool withdrawal, TimePoint now)
        {
            var eventDay = now.Day.AddDays(1);
            var valueDay = now.Day.AddDays(3);
            return new CashInOut {
                AccountId = accountId,
                Currency = "JPY",
                AbsAmount = Decimal.Parse(absAmount),
                Withdrawal = withdrawal,
                RequestDay = now.Day,
                RequestDate = now.Date,
                EventDay = eventDay,
                ValueDay = valueDay,
                TargetFiCode = "tFiCode",
                TargetFiAccountId = "tFiAccId",
                SelfFiCode = "sFiCode",
                SelfFiAccountId = "sFiAccId",
                StatusType = ActionStatusType.Unprocessed,
                UpdateDate = now.Date
            };
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