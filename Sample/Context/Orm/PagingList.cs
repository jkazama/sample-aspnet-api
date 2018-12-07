using System.Collections.Generic;

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
