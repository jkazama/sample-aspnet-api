using Sample.Context;

namespace Sample.Models.Master
{
    //<summary>
    // 社員を表現します。
    //</summary>
    public class Staff
    {
        /** ID */
        public string Id { get; set; }
        /** 名前 */
        public string Name { get; set; }
        /** パスワード(暗号化済) */
        public string Password { get; set; }

        public Actor actor()
        {
            return new Actor { Id = Id, Name = Name, RoleType = ActorRoleType.Internal };
        }
    }
}