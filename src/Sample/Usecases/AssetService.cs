
using Sample.Models;

namespace Sample.Usecases
{
    public class AssetService
    {
        private Repository _rep;
        public AssetService(Repository rep)
        {
            _rep = rep;
        }

        public string Hello()
        {
            // using (var tx = _infra.models.Database.BeginTransaction())
            // {
            //     try
            //     {
            //         tx.Commit();
            //         return "はろ！";
            //     }
            //     catch (Exception)
            //     {
            //         tx.Rollback();
            //         throw;
            //     }
            // }
            return "";
        }
    }
}