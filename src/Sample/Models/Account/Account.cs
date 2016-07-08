using System;
using System.Collections.Generic;
using System.Linq;
using Sample.Context;
using Sample.Context.Orm;
using Sample.Utils;

namespace Sample.Models.Account
{
    //<summary>
    // 口座を表現します。
    // low: サンプル用に必要最低限の項目だけ口座状態を表現します
    //</summary>
    public class Account : OrmActiveRecord<Account>
    {
        /** 口座ID */
        public string Id { get; set; }
        /** 口座名義 */
        public string Name { get; set; }
        /** メールアドレス */
        public string Mail { get; set; }
        /** 口座状態 */
        public AccountStatusType StatusType { get; set; }

        //<summary>口座に紐付くログイン情報を取得します。</summary>
        public Actor Actor()
        {
            return new Actor { Id = Id, Name = Name, RoleType = ActorRoleType.User };
        }
        //<summary>口座に紐付くログイン情報を取得します。</summary>
        public Login LoadLogin(Repository rep)
        {
            return Login.Load(rep, Id);
        }
        //<summary>口座を変更します。</summary>
        public Account Change(Repository rep, ChgAccount p)
        {
            return p.Bind(this).Update(rep);
        }

        //<summary>口座を取得します。</summary>
        public static Account Get(Repository rep, string id)
        {
            return rep.Get<Account>(m => m.Id == id);
        }
        //<summary>有効な口座を取得します。</summary>
        public static Account GetValid(Repository rep, string id)
        {
            return rep.Load<Account>(m => m.Id == id && m.StatusType == AccountStatusType.Normal);
        }
        //<summary>口座を取得します。(例外付)</summary>
        public static Account Load(Repository rep, string id)
        {
            return rep.Load<Account>(m => m.Id == id);
        }
        //<summary>有効な口座を取得します。(例外付)</summary>
        public static Account LoadValid(Repository rep, string id)
        {
            return rep.Load<Account>(m => m.Id == id && m.StatusType == AccountStatusType.Normal);
        }
        //<summary>
        //口座の登録を行います。
        //<p>ログイン情報も同時に登録されます。
        //</summary>
        public static Account Register(Repository rep, RegAccount p)
        {
            Validator.Validate(v => v.CheckField(Get(rep, p.Id) == null, "id", Resources.Exception.DuplicateId));
            p.CreateLogin(p.PlainPassword).Save(rep); //TODO: ApplicationUserの登録
            return p.Create().Save(rep);
        }
    }

    //<summary>口座状態を表現します</summary>
    public enum AccountStatusType
    {
        /** 通常 */
        Normal,
        /** 退会 */
        Withdrwal
    }
    public static class AccountStatusTypes
    {
        //<summary>口座状態一覧</summary>
        public static IList<AccountStatusType> All =
            Enum.GetValues(typeof(AccountStatusType)).Cast<AccountStatusType>().ToList();
        public static bool Valid(this AccountStatusType statusType) { return statusType == AccountStatusType.Normal; }
        public static bool Invalid(this AccountStatusType statusType) { return !Valid(statusType); }
    }

    //<summary>登録パラメタ</summary>
    public class RegAccount : IDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        /** パスワード(未ハッシュ) */
        public string PlainPassword { get; set; }

        public Account Create()
        {
            return new Account { Id = Id, Name = Name, Mail = Mail, StatusType = AccountStatusType.Normal };
        }
        public Login CreateLogin(string password)
        {
            return new Login { Id = Id, LoginId = Id, Password = password };
        }
    }
    //<summary>変更パラメタ</summary>
    public class ChgAccount : IDto
    {
        public string Name { get; set; }
        public string Mail { get; set; }

        public Account Bind(Account m)
        {
            m.Name = Name;
            m.Mail = Mail;
            return m;
        }
    }
}