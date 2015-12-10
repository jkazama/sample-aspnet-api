using System.Threading;

namespace Sample.Context
{
    //<summary>
    // 非同期メソッドスコープの利用者セッション
    // <p>ThreadLocalと異なりネストした非同期処理内で変数をスタックして管理します。
    // 子階層での変数上書きは親階層に反映されないので取り扱いに注意してください。
    //</summary>
    public class ActorSession
    {
        private AsyncLocal<Actor> _local = new AsyncLocal<Actor>();

        // <summary>利用者セッションへ利用者を紐付けます</summary>
        public ActorSession bind(Actor actor) {
            _local.Value = actor;
            return this;
        }
        // <summary>利用者セッションを破棄します</summary>
        public ActorSession unbind() {
            _local.Value = null;
            return this;
        }
        // <summary>有効な利用者を返します。紐付けされていない時は匿名者が返されます。</summary>
        public Actor actor() {
            return _local.Value != null ? _local.Value : Actor.Anonymous;
        }
    }
}