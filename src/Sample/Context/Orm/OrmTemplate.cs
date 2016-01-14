using System.Data;
using System.Data.Common;
using Microsoft.Data.Entity;

namespace Sample.Context.Orm
{
    public class OrmTemplate
    {
        private OrmRepository _rep;
        public OrmTemplate(OrmRepository rep)
        {
            _rep = rep;
        }
        public async void FindBySql(string sql, DbParameter[] args)
        {
            using(var cmd = _rep.Database.GetDbConnection().CreateCommand()) {
                cmd.CommandText = sql;
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                // cmd.Parameters.Add = args;
                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    //todo
                }
            }
        }
    }
}