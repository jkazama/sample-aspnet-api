using Sample.Context;
using Sample.Context.Orm;

namespace Sample.Models.Account
{
    //<summary>
    // 口座ログインを表現します。
    // low: サンプル用に必要最低限の項目だけ口座状態を表現します
    // low: ApplicationUserとの統合
    //</summary>
    public class Login : OrmActiveRecord<Login>
    {
        /** 口座ID */
        public string Id { get; set; }
        /** ログインID */
        public string LoginId { get; set; }
        /** パスワード(暗号化済) */
        public string Password { get; set; }

        //<summary>ログインIDを変更します</summary>
        public Login Change(Repository rep, ChgLoginId p)
        {
            bool exists = rep.Get<Login>(m => m.Id != Id && m.LoginId == p.LoginId) != null;
            Validate(v => v.CheckField(!exists, "loginId", Resources.Exception.DuplicateId));
            return p.Bind(this).Update(rep);
        }
        //<summary>パスワードを変更します</summary>
        public Login Change(Repository rep, ChgPassword p)
        {
            return p.Bind(this, p.PlainPassword).Update(rep); //TODO: 暗号化
        }

        /** ログイン情報を取得します */
        public static Login Get(Repository rep, string id)
        {
            return rep.Get<Login>(m => m.Id == id);
        }
        /** ログイン情報を取得します */
        public static Login GetByLoginId(Repository rep, string loginId)
        {
            return rep.Get<Login>(m => m.LoginId == loginId);
        }
        /** ログイン情報を取得します。(例外付) */
        public static Login Load(Repository rep, string id)
        {
            return rep.Load<Login>(m => m.Id == id);
        }
    }

    //<summary>ログインID変更パラメタ low: 基本はユースケース単位で切り出す</summary>
    public class ChgLoginId : IDto
    {
        public string LoginId { get; set; }
        public Login Bind(Login m)
        {
            m.LoginId = LoginId;
            return m;
        }
    }
    //<summary>パスワード変更パラメタ</summary>
    public class ChgPassword : IDto
    {
        public string PlainPassword { get; set; }
        public Login Bind(Login m, string password)
        {
            m.Password = password;
            return m;
        }
    }

}