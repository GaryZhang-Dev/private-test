using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.infrastructure.Extensions
{
    public class MigurationStartupTask<TDbContext> : IStartupTask where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public MigurationStartupTask(TDbContext dbContext){
            _dbContext = dbContext;
        }

        public void Run() {
            _dbContext.Database.Migrate();
        }
    }
}
