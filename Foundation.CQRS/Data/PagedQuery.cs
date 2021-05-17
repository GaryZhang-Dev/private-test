using System.Collections.Generic;
using Foundation.CQRS;

namespace Foundation.Data
{
    public abstract class PagedQuery<TResultListItem> : IQuery<PagedList<TResultListItem>>, IPagedQuery
    {
        public int Page { get; set; } = 1;
        public virtual int PageSize { get; set; } = 10;
    }

    public abstract class Query<TResultListItem> : IQuery<List<TResultListItem>>
    {
    }
}
