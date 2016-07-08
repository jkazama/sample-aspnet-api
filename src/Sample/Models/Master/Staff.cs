using System.Collections.Generic;
using System.Linq;
using Sample.Context;
using Sample.Context.Orm;
using Sample.Utils;

namespace Sample.Models.Master
{
    //<summary>
    // 社員を表現します。
    //</summary>
    public class Staff : OrmActiveRecord<Staff>
    {
        /** ID */
        public string Id { get; set; }
        /** 名前 */
        public string Name { get; set; }
        /** パスワード(暗号化済) */
        public string Password { get; set; }

        public Actor Actor()
        {
            return new Actor { Id = Id, Name = Name, RoleType = ActorRoleType.Internal };
        }
        //<summary>パスワードを変更します</summary>
        public Staff Change(Repository rep, ChgPassword p)
        {
            return p.Bind(this, p.PlainPassword).Update(rep); //TODO: 暗号化
        }
        //<summary>社員情報を変更します</summary>
        public Staff Change(Repository rep, ChgStaff p)
        {
            return p.Bind(this).Update(rep);
        }
        //<summary>社員を取得します</summary>
        public static Staff Get(Repository rep, string id)
        {
            return rep.Get<Staff>(m => m.Id == id);
        }
        //<summary>社員を取得します(例外付)</summary>
        public static Staff Load(Repository rep, string id)
        {
            return rep.Load<Staff>(m => m.Id == id);
        }
        //<summary>社員を検索します</summary>
        public static List<Staff> Find(Repository rep, FindStaff p)
        {
            return rep.Template<Staff>().Find(
                m => m.Id.Contains(p.Keyword) || m.Name.Contains(p.Keyword),
                query => query.OrderBy(m => m.Id));
        }
        //<summary>社員を登録します</summary>
        public static Staff Register(Repository rep, RegStaff p)
        {
            Validator.Validate(v => v.CheckField(Staff.Get(rep, p.Id) == null, "id", Resources.Exception.DuplicateId));
            return p.Create(p.PlainPassword).Save(rep); //TODO: 暗号化
        }
    }

    //<summary>検索パラメタ</summary>
    public class FindStaff : IDto
    {
        public string Keyword { get; set; }
    }

    //<summary>登録パラメタ</summary>
    public class RegStaff : IDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        /** パスワード（未ハッシュ） */
        public string PlainPassword { get; set; }
        public Staff Create(string password)
        {
            return new Staff { Id = Id, Name = Name, Password = password };
        }
    }

    //<summary>変更パラメタ</summary>
    public class ChgStaff : IDto
    {
        public string Name { get; set; }
        public Staff Bind(Staff m)
        {
            m.Name = Name;
            return m;
        }
    }

    //<summary>パスワード変更パラメタ</summary>
    public class ChgPassword : IDto
    {
        /** パスワード（未ハッシュ） */
        public string PlainPassword { get; set; }
        public Staff Bind(Staff m, string password)
        {
            m.Password = password;
            return m;
        }
    }
}