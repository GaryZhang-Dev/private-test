using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundation.Data
{

    public interface IPagedList { 
        int StartPage { get; }

        int EndPage { get;}

        int Page { get; }

        int PageSize { get; set; }

        int Total { get; }

        int StartNum { get; }

        int EndNum { get; }

        int TotalPage { get; set; }
    }

    public interface IPagedQuery { 
        int Page { get; }

        int PageSize { get; }
    }
    public class PagedList<TData> : IEnumerable<TData>, IPagedList
    {
        public PagedList(int page,int pageSize,int total,IEnumerable<TData> data){
            Page = page;
            PageSize = pageSize;
            Total = total;
            Data = data;
            TotalPage = (int)Math.Ceiling((double)Total / pageSize);
            StartPage = (Page - 3 < 1) ? 1 : Page - 3;
            EndPage = (StartPage + 6 > TotalPage) ? TotalPage : StartPage + 6;
            StartNum = (Page - 1) * PageSize + 1;
            EndNum = (Page - 1) * PageSize + PageSize;
        }

        public int StartPage { get; private set; }

        public int EndPage { get; private set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int Total { get; private set; }

        public int StartNum { get;}

        public int EndNum { get; }

        public int TotalPage { get; set; }

        public IEnumerable<TData> Data { get; set; }

        public IEnumerator<TData> GetEnumerator() => Data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Data.GetEnumerator();

        public object ToJsonObject() => new { Total, Items = Data.ToList() };
        public object ToJsonObject(Func<TData, object> selector) => new { Total, Items = Data.Select(selector).ToList() };
    }
}
