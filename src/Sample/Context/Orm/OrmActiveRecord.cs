using System;
using Sample.Utils;

namespace Sample.Context.Orm
{
    //<summary>
    // ORMベースでActiveRecordの概念を提供するEntity基底クラス。
    // <p>ここでは自インスタンスの状態に依存する簡易な振る舞いのみをサポートします。
    // 実際のActiveRecordモデルにはget/find等の概念も含まれますが、それらは 自己の状態を
    // 変える行為ではなく対象インスタンスを特定する行為(クラス概念)にあたるため、
    // クラスメソッドとして継承先で個別定義するようにしてください。
    //</summary>
    public abstract class OrmActiveRecord<TEntity> : IEntity where TEntity : class
    {
        //<summary>審査処理をします。</summary>
        protected TEntity Validate(Action<Validator> act)
        {
            Validator.Validate(act);
            return this as TEntity;
        }
        //<summary>与えられたレポジトリを経由して自身を新規追加します。</summary>
        public TEntity Save(OrmRepository rep)
        {
            return rep.Save<TEntity>(this as TEntity);
        }
        //<summary>与えられたレポジトリを経由して自身を更新します。</summary>
        public TEntity Update(OrmRepository rep)
        {
            return rep.Change<TEntity>(this as TEntity);
        }
        //<summary>与えられたレポジトリを経由して自身を物理削除します。</summary>
        public TEntity Delete(OrmRepository rep)
        {
            return rep.Delete<TEntity>(this as TEntity);
        }
        //<summary>与えられたレポジトリを経由して自身を新規追加または更新します。</summary>
        public TEntity SaveOrUpdate(OrmRepository rep)
        {
            return rep.SaveOrUpdate<TEntity>(this as TEntity);
        }
    }
}