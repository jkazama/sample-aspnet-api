namespace Sample.Models.Account
{
    //<summary>
    // 口座ログインを表現します。
    // low: サンプル用に必要最低限の項目だけ口座状態を表現します
    //</summary>
    public class Login
    {
        /** 口座ID */
        public string Id { get; set; }
        /** ログインID */
        public string LoginId { get; set; }
        /** パスワード(暗号化済) */
        public string Password { get; set; }
    }

}