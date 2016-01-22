using Sample.Models;
using Sample.Models.Account;

namespace Sample.Usecases
{
    //<summary>
    // 口座ドメインに対する顧客ユースケース処理
    //</summary>
    public class AccountService
    {
        private readonly Repository _rep;
        public AccountService(Repository rep) {
            _rep = rep;
        }

        //<summary>ログイン情報を取得します</summary>
        public Login GetLoginByLoginId(string loginId)
        {
            return _rep.Tx(() => Login.GetByLoginId(_rep, loginId));
        }

        //<summary>有効な口座情報を取得します</summary>
        public Account GetAccount(string id)
        {
            return _rep.Tx(() => Account.GetValid(_rep, id));
        }
    }
}