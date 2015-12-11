using Microsoft.Data.Entity;
using Sample.Context;
using Sample.Context.Orm;
using Sample.Models.Account;

namespace Sample.Models
{

    //<summary>
    // Entity Frameworkが提供するモデル永続化アクセサ
    //</summary>
    public class Repository : OrmRepository
    {

        public Repository(DomainHelper dh) : base(dh) { }
        
    }
}