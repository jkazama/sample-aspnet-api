using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Context.Orm
{
    //<summary>
    // DbCommandに対する簡易アクセサです。
    // <p>DbCommandをビルダー形式で構築可能になります。
    //</summary>
    public class OrmDbCommand
    {
        public DbCommand DbCommand { get; set; }
        private OrmDbCommand(DbCommand cmd)
        {
            this.DbCommand = cmd;
        }

        //<summary>SQLを設定します。</summary>
        public OrmDbCommand Sql(string sql)
        {
            this.DbCommand.CommandText = sql;
            return this;
        }

        //<summary>
        // パラメータを設定します。
        // <p> @p0 @p1 といった順序文字列でSQL句へバインドさせることが可能です。
        //</summary>
        public OrmDbCommand Parameters(params object[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                var param = this.DbCommand.CreateParameter();
                param.ParameterName = "@p" + i;
                param.Value = parameters[i];
                this.DbCommand.Parameters.Add(param);
            }
            return this;
        }

        //<summary>
        // 名前付パラメータを設定します。
        // <p> @キー名称 でSQL句へバインドさせることが可能です。
        //</summary>
        public OrmDbCommand Parameters(params KeyValuePair<string, object>[] parameters)
        {
            foreach (var p in parameters)
            {
                var param = this.DbCommand.CreateParameter();
                param.ParameterName = p.Key;
                param.Value = p.Value;
                this.DbCommand.Parameters.Add(param);
            }
            return this;
        }

        //<summary>
        // 検索クエリを実行して戻り値を取ります。
        // <p>事前にSqlメソッド呼び出しておく必要があります。
        // <p>戻り値一覧内のオブジェクトは行カラム名のキーで値を取得する事が可能です。
        //</summary>
        public List<dynamic> Find()
        {
            var list = new List<dynamic>();
            using (var reader = this.DbCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    var row = new ExpandoObject() as IDictionary<string, object>;
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetName(i), reader[i]);
                    }
                    list.Add(row);
                }
            }
            return list;
        }

        //<summary>
        // 更新クエリを実行します。
        // <p>事前にSqlメソッド呼び出しておく必要があります。
        //</summary>
        public int Execute()
        {
            return this.DbCommand.ExecuteNonQuery();
        }

        //<summary>DbCommandを元にOrmDbCommandを生成します。</summary>
        public static OrmDbCommand Of(DbCommand cmd)
        {
            return new OrmDbCommand(cmd);
        }
    }
}
