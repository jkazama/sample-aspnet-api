using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using System.Data;
using System.Dynamic;
using System.Data.Common;

namespace Sample.Context.Orm
{
    //<summary>
    // Entity Frameworkが提供するモデル永続化アクセサ
    // <p>プロジェクトが利用するスキーマ毎に本クラスを継承したRepositoryを作成してください。
    // <p>スキーマ生成概念も含めたい時は継承先で忘れずにDbSet<[Entity]>をEntity分定義するようにしてください。
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

        //<summary>インフラ層のユーティリティヘルパ</summary>
        public DomainHelper Helper { get; set; }

        //<summary>
        // 複雑な条件検索などをサポートするOrmTemplateを生成します。
        //</summary>
        public OrmTemplate<TEntity> Template<TEntity>() where TEntity : class
        {
            return new OrmTemplate<TEntity>(this);
        }

        //<summary>
        // OrmTemplateで利用される条件句構築ビルダーを生成します。
        //</summary>
        public OrmCriteria<TEntity> Criteria<TEntity>() where TEntity : class
        {
            return new OrmCriteria<TEntity>();
        }

        //<summary>
        // 指定したEntityクラスのDbSetを取得します。
        //</summary>
        public DbSet<TEntity> EntitySet<TEntity>() where TEntity : class
        {
            return this.Set<TEntity>();
        }

        //<summary>
        // 戻り値を持たないトランザクション処理を実行します。
        //</summary>
        public void Tx(Action fn)
        {
            this.Tx<object>(() => { fn(); return true; });
        }

        //<summary>
        // 戻り値を必要とするトランザクション処理を実行します。
        //</summary>
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

        //<summary>
        // Orm経由での更新処理を確定させます。
        //</summary>
        public void Flush()
        {
            SaveChanges();
        }

        //<summary>
        // 指定したLINQ句で一件を取得します。
        // <p>存在しない時はnullが返されます。
        //</summary>
        public TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return EntitySet<TEntity>().FirstOrDefault(predicate);
        }

        //<summary>
        // 指定したLINQ句で一件を取得します。
        // <p>存在しない時は例外が返されます。
        //</summary>
        public TEntity Load<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            TEntity v = Get(predicate);
            if (v != null) return v;
            else throw new ValidationException(ErrorKeys.EntityNotFound);
        }
        //<summary>
        // 指定したLINQ句に合致するEntityが存在する時はtrue。
        //</summary>
        public bool Exists<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return Get<TEntity>(predicate) != null;
        }

        //<summary>
        // 全てのEntityを取得します。
        //</summary>
        public List<TEntity> FindAll<TEntity>() where TEntity : class
        {
            return EntitySet<TEntity>().ToList();
        }

        //<summary>
        // Entityを新規追加します。
        // <p>SaveChangesは自動的に呼び出されます。
        //</summary>
        public TEntity Save<TEntity>(TEntity entity) where TEntity : class
        {
            EntitySet<TEntity>().Add(entity);
            SaveChanges();
            return entity;
        }

        //<summary>
        // Entityを変更します。
        // <p>SaveChangesは自動的に呼び出されます。
        //</summary>
        public TEntity Update<TEntity>(TEntity entity) where TEntity : class
        {
            EntitySet<TEntity>().Update(entity);
            SaveChanges();
            return entity;
        }

        //<summary>
        // Entityを削除します。
        // <p>SaveChangesは自動的に呼び出されます。
        //</summary>
        public TEntity Delete<TEntity>(TEntity entity) where TEntity : class
        {
            EntitySet<TEntity>().Remove(entity);
            SaveChanges();
            return entity;
        }

        //<summary>
        // Entityを新規追加または更新します。
        // <p>SaveChangesは自動的に呼び出されます。
        //</summary>
        public TEntity SaveOrUpdate<TEntity>(TEntity entity) where TEntity : class
        {
            if (Entry(entity).State == EntityState.Detached)
            {
                EntitySet<TEntity>().Add(entity);
            }
            SaveChanges();
            return entity;
        }

        //<summary>
        // ORMが管理するデータベースに対する直接的な操作を行います。
        //</summary>
        public TResult DbAction<TResult>(Func<OrmDbCommand, TResult> fn)
        {
            using (var cmd = Database.GetDbConnection().CreateCommand())
            {
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                return fn(OrmDbCommand.Of(cmd));
            }
        }

        //<summary>
        // 参照SQLを実行します。
        // <p> @p0 @p1 といった順序文字列で指定したパラメタ値をSQL句へバインドさせることが可能です。
        //</summary>
        public List<dynamic> FindSql(string sql, params object[] parameters)
        {
            return DbAction<List<dynamic>>(cmd =>
                cmd.Sql(sql).Parameters(parameters).Find());
        }

        //<summary>
        // 更新SQLを実行します。
        // // <p> @p0 @p1 といった順序文字列で指定したパラメタ値をSQL句へバインドさせることが可能です。
        //</summary>
        public int ExecuteSql(string sql, params object[] parameters)
        {
            return DbAction<int>(cmd =>
                cmd.Sql(sql).Parameters(parameters).Execute());
        }

    }

}