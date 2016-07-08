using Sample.Models;

namespace Sample.Usecases
{
    //<summary>
    // システムドメインに対する社内ユースケース処理。
    //</summary>
    public class SystemAdminService
    {
        private Repository _rep;

        public SystemAdminService(Repository rep)
        {
            this._rep = rep;
        }

    }
}
