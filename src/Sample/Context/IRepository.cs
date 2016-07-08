using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sample.Context
{
    //<summary>
    // 特定のドメインオブジェクトに依存しない汎用的なRepositoryです。
    // <p>タイプセーフでないRepositoryとして利用することができます。
    //</summary>
    interface IRepository
    {
        //<summary>
        // ドメイン層においてインフラ層コンポーネントへのアクセスを提供するヘルパーユーティリティを返します。
        //</summary>
        DomainHelper Helper { get; }

        //<summary>
        // 指定した条件句に応じた1件のEntityを取得します。(存在しない時はnull)
        //</summary>
        TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        //<summary>
        // 指定した条件句に応じた1件のEntityを取得します。(存在しない時は例外)
        //</summary>
        TEntity Load<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        //<summary>
        // 管理するEntityを全件返します。
        //</summary>
        List<TEntity> FindAll<TEntity>() where TEntity : class;

        //<summary>
        // Entityを新規追加します。
        //</summary>
        TEntity Save<TEntity>(TEntity entity) where TEntity : class;

        //<summary>
        // Entityを更新します。
        //</summary>
        TEntity Change<TEntity>(TEntity entity) where TEntity : class;

        //<summary>
        // Entityを削除します。
        //</summary>
        TEntity Delete<TEntity>(TEntity entity) where TEntity : class;
    }
}