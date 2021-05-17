using Foundation.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.CQRS.Data
{
    public static class EntityFrameworkCoreExtensions
    {
        public static async Task<PagedList<TSource>> ToPagedListAsync<TSource>(this IQueryable<TSource> @this, int page, int pageSize) { 
            
            return new PagedList<TSource>(page,pageSize,await @this.CountAsync(),await @this.Skip((page-1)*pageSize).Take(pageSize).ToListAsync());
        }
        public static Task<PagedList<TSource>> ToPagedListAsync<TSource>(this IQueryable<TSource> @this, IPagedQuery query) => @this.ToPagedListAsync(query.Page, query.PageSize);
    }
}
