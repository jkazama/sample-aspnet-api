using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sample.Context.Orm
{
    //<summary>
    // ORM向けの条件句LinqExpression(Whereのみを対象)を構築します。
    // 動的概念を含むLinqExpressionの簡易的な取り扱いを可能にします。
    //</summary>
    public class OrmCriteria<TEntity> where TEntity : class
    {
        private List<Expression<Func<TEntity, bool>>> _predicates = new List<Expression<Func<TEntity, bool>>>();

        //<summary>LINQ条件句を追加します</summary>
        public OrmCriteria<TEntity> Add(Expression<Func<TEntity, bool>> predicate)
        {
            _predicates.Add(predicate);
            return this;
        }

        //<summary>valid が有効な時のみLINQ条件句を追加します。</summary>
        public OrmCriteria<TEntity> Add(bool valid, Expression<Func<TEntity, bool>> predicate)
        {
            return valid ? Add(predicate) : this;
        }

        //<summary>valid が有効/無効それぞれに応じたLINQ条件句を追加します。</summary>
        public OrmCriteria<TEntity> Add(bool valid, Expression<Func<TEntity, bool>> truePredicate, Expression<Func<TEntity, bool>> falsePredicate)
        {
            return valid ? Add(truePredicate) : Add(falsePredicate);
        }

        //<summary>蓄積されたLINQ条件句を返します。</summary>
        public List<Expression<Func<TEntity, bool>>> Predicates()
        {
            return _predicates;
        }
    }
}
