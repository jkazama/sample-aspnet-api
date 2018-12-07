using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Sample.Context;
using Sample.Context.Orm;

namespace Sample.Models
{

    //<summary>
    // Entity Frameworkが提供するモデル永続化アクセサ
    //</summary>
    public class Repository : OrmRepository
    {
        public Repository(DbContextOptions options, DomainHelper dh) : base(options, dh) { }
        public Repository(DomainHelper dh) : base(dh) { }

        public DbSet<Account.Account> Accounts { get; set; }
        public DbSet<Account.FiAccount> FiAccounts { get; set; }
        public DbSet<Account.Login> Logins { get; set; }

        public DbSet<Asset.CashBalance> CashBalances { get; set; }
        public DbSet<Asset.Cashflow> Cashflows { get; set; }
        public DbSet<Asset.CashInOut> CashInOuts { get; set; }

        public DbSet<Master.Holiday> Holidays { get; set; }
        public DbSet<Master.SelfFiAccount> SelfFiAccounts { get; set; }
        public DbSet<Master.Staff> Staffs { get; set; }
        public DbSet<Master.StaffAuthority> StaffAuthorities { get; set; }
    }
}