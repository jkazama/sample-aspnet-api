namespace Sample.Context
{
    //<summary>
    // ドメイン処理を行う上で必要となるインフラ層コンポーネントへのアクセサを提供します。
    //</summary>
    public class DomainHelper
    {
        /** スレッドローカルスコープの利用者セッションを取得します。 */
        public ActorSession ActorSession { get; set; }
        /** 日時ユーティリティを取得します。 */
        public Timestamper Time { get; set; }

        public DomainHelper()
        {
            this.ActorSession = new ActorSession();
            this.Time = new Timestamper();
        }

        //<summary>
        // ログイン中のユースケース利用者を取得します。
        //</summary>
        public Actor Actor()
        {
            return ActorSession.Actor();
        }
    }
}