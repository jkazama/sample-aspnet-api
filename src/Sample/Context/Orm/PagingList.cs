using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Context.Orm
{
    //<summary>
    // ページング一覧を表現します。
    //</summary>
    public class PagingList<TEntity> : IDto where TEntity : class
    {
        public List<TEntity> List { get; set; }
        public Pagination Page { get; set; }
    }
}
