using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;

namespace Sample.Context.Orm
{
    //<summary>
    // Entity Frameworkが提供するモデル永続化アクセサ
    // todo: リソースリーク絡みの挙動を調べる
    // todo: SQL実行系のアプローチを調べる
    //</summary>
    public abstract class OrmRepository : DbContext, IRepository
    {
        public OrmRepository(DomainHelper Helper)
        {
            this.Helper = Helper;
        }
        public OrmRepository(DbContextOptions options, DomainHelper Helper) : base(options)
        {
            this.Helper = Helper;
        }

        public DomainHelper Helper { get; set; }

        public OrmTemplate Template()
        {
            return new OrmTemplate(this);
        }

        public DbSet<TEntity> EntitySet<TEntity>() where TEntity : class
        {
            return this.Set<TEntity>();
        }

        public void Tx(Action fn)
        {
            this.Tx<object>(() => { fn(); return true; });
        }

        public TResult Tx<TResult>(Func<TResult> fn)
        {
            using (var transaction = this.Database.BeginTransaction())
            {
                var ret = fn();
                Flush();
                transaction.Commit();
                return ret;
            }
        }

        public void Flush()
        {
            SaveChanges();
        }

        public TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return EntitySet<TEntity>().Where(predicate).FirstOrDefault();
        }

        public TEntity Load<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            TEntity v = Get(predicate);
            if (v != null) return v;
            else throw new ValidationException(ErrorKeys.EntityNotFound);
        }

        public List<TEntity> FindAll<TEntity>() where TEntity : class
        {
            return EntitySet<TEntity>().ToList();
        }

        public TEntity Save<TEntity>(TEntity entity) where TEntity : class
        {
            EntitySet<TEntity>().Add(entity);
            SaveChanges();
            return entity;
        }
        public TEntity Update<TEntity>(TEntity entity) where TEntity : class
        {
            EntitySet<TEntity>().Update(entity);
            SaveChanges();
            return entity;
        }
        public TEntity Delete<TEntity>(TEntity entity) where TEntity : class
        {
            EntitySet<TEntity>().Remove(entity);
            SaveChanges();
            return entity;
        }
        public TEntity SaveOrUpdate<TEntity>(TEntity entity) where TEntity : class
        {
            if (Entry(entity).State == EntityState.Detached)
            {
                EntitySet<TEntity>().Add(entity);
            }
            SaveChanges();
            return entity;
        }
    }

}