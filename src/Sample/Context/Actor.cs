namespace Sample.Context
{
    //<summary>
    // ユースケースにおける利用者を表現します。
    //</summary>
    public class Actor : IDto
    {
        /** 匿名利用者定数 */
        public static Actor Anonymous = new Actor { Id = "unknown", Name = "unknown", RoleType = ActorRoleType.Anonymous };
        /** システム利用者定数 */
        public static Actor System = new Actor { Id = "system", Name = "system", RoleType = ActorRoleType.System };

        /** 利用者ID */
        public string Id { get; set; }
        /** 利用者名称 */
        public string Name { get; set; }
        /** 利用者が持つActorRoleType */
        public ActorRoleType RoleType { get; set; }
        /** 利用者の接続チャネル名称 */
        public string Channel { get; set; }
        /** 利用者を特定する外部情報 */
        public string Source { get; set; }
    }

    public enum ActorRoleType
    {
        /** 匿名利用者 */
        Anonymous,
        /** 利用者(主にBtoCの顧客, BtoB提供先社員) */
        User,
        /** 内部利用者(主にBtoCの社員, BtoB提供元社員) */
        Internal,
        /** システム管理者(ITシステム担当社員またはシステム管理会社の社員) */
        Administrator,
        /** システム(システム上の自動処理) */
        System
    }

}