using AccountService.Api.Queries;
using AccountService.Domain;
using AccountService.Domain.Models;
using Foundation.CQRS.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Api.Controllers
{
    public class TestController : Controller
    {

        private AccountDbContext _dbContext { get;set; }
        public TestController(AccountDbContext dbContext) {
            _dbContext = dbContext;
        }
        [HttpPost("api/test-post")]
        public async Task<object> TestAcctount()
        {
            return Ok("Test Successful");
        }

        [HttpPost("api/books")]
        public async Task<object> GetBooks(BooksPagedQuery query) {
            var books = _dbContext.Set<Books>().AsQueryable();

            var result = await books.OrderByDescending(x => x.CreatedOn).ToPagedListAsync(query);
            return result;
        }

        
    }
}
