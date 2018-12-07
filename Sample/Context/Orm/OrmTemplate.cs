using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;

namespace Sample.Context.Orm
{
    //<summary>
    // OrmRepositoryが管理している特定のDbSetに対する簡易アクセサ。
    // セッション毎に生成して利用してください。
    // <p>プロジェクト単位で必要に応じて拡張してください
    //</summary>
    public class OrmTemplate<TEntity> where TEntity : class
    {
        private OrmRepository _rep;
        private DbSet<TEntity> EntitySet { get; set; }
        public OrmTemplate(OrmRepository rep)
        {
            this._rep = rep;
            this.EntitySet = rep.EntitySet<TEntity>();
        }

        //<summary>DbSet<TEntity>.Where 同等の振る舞いをします。</summary>
        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return EntitySet.Where(predicate);
        }

        //<summary>スタックしたLINQ句を全てWhere指定します。動的条件句などで利用してください。</summary>
        public IQueryable<TEntity> Where(List<Expression<Func<TEntity, bool>>> predicates)
        {
            if (predicates.Count == 0) return EntitySet.Where(m => true);
            var query = EntitySet.Where(predicates[0]);
            for (int i = 0; i < predicates.Count; i++)
            {
                if (0 < i) query = query.Where(predicates[i]);
            }
            return query;
        }

        //<summary>与えられたLINQ句を元にカウント集計をおこないます。</summary>
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return Where(predicate).Count();
        }

        //<summary>与えられたLINQ句を元にカウント集計をおこないます。動的条件句などで利用してください。</summary>
        public int Count(List<Expression<Func<TEntity, bool>>> predicates)
        {
            return Where(predicates).Count();
        }

        //<summary>
        // 与えられたLINQ句を元に単純な検索をおこないます。
        // <p>第2引数でIQueryableに対する追加指定が可能です。
        // OrderByなどを指定したいときはこちらを利用してください。
        //</summary>
        public List<TEntity> Find(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> appendQuery = null)
        {
            var query = Where(predicate);
            if (appendQuery != null) query = appendQuery(query);
            return query.ToList();
        }

        //<summary>
        // 与えられたスタックLINQ句を元に単純な検索をおこないます。
        // <p>LINQ句を全てWhere指定の引数へ与えられます。動的条件句などで利用してください。
        // <p>第2引数でIQueryableに対する追加指定が可能です。
        // OrderByなどを指定したいときはこちらを利用してください。
        //</summary>
        public List<TEntity> Find(List<Expression<Func<TEntity, bool>>> predicates, Func<IQueryable<TEntity>, IQueryable<TEntity>> appendQuery = null)
        {
            var query = Where(predicates);
            if (appendQuery != null) query = appendQuery(query);
            return query.ToList();
        }

        //<summary>
        // 与えられたLINQ句を元に単純なページング検索をおこないます。
        // <p>第2引数でIQueryableに対する追加指定が可能です。
        // OrderByなどを指定したいときはこちらを利用してください。
        //</summary>
        public PagingList<TEntity> Find(Pagination page, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> appendQuery = null)
        {
            return Find(page, new List<Expression<Func<TEntity, bool>>> { predicate }, appendQuery);
        }

        //<summary>
        // 与えられたスタックLINQ句を元に単純なページング検索をおこないます。
        // <p>LINQ句を全てWhere指定の引数へ与えられます。動的条件句などで利用してください。
        // <p>第2引数でIQueryableに対する追加指定が可能です。
        // OrderByなどを指定したいときはこちらを利用してください。
        //</summary>
        public PagingList<TEntity> Find(Pagination page, List<Expression<Func<TEntity, bool>>> predicates, Func<IQueryable<TEntity>, IQueryable<TEntity>> appendQuery = null)
        {
            var totalPage = page.IgnoreTotal ? page : page.CopyWithTotal(Count(predicates));
            var query = Where(predicates);
            if (appendQuery != null) query = appendQuery(query);
            return new PagingList<TEntity>
            {
                List = query.Skip(page.FirstResult()).Take(page.Size).ToList(),
                Page = totalPage
            };
        }

        //<summary>
        // Entityに関連させるSQLを実行します。
        // <p>パラメタ引数指定時は @p0 @p1 といった順序文字列でSQL句へバインドさせることが可能です。
        // <p>Entityに紐付けする必要が無いときは OrmRepository#FindSql, OrmRepository#ExecuteSql を利用してください。
        //</summary>
        public IList<TEntity> FindSql(string sql, params object[] parameters)
        {
            return EntitySet.FromSql(sql, parameters).ToList();
        }

    }
}