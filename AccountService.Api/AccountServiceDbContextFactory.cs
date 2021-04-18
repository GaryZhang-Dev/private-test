using AccountService.Domain;
using Foundation.CQRS.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountService.Api
{
    public class AccountServiceDbContextFactory : IDesignTimeDbContextFactory<AccountDbContext>
    {
        public AccountDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder();
            var configuration = config.AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("appsettings." + (Environments.EnvironmentName.IsNullOrEmpty() ? "Development" : Environments.EnvironmentName) + ".json", true, true)
                .AddEnvironmentVariables().Build();
            Console.WriteLine("ConnectionString: " + configuration["AccountCenterDb"]);

            var optionsBuilder = new DbContextOptionsBuilder<AccountDbContext>();
            optionsBuilder.UseSqlServer(configuration["AccountCenterDb"],
                option => option.MigrationsAssembly(typeof(AccountServiceDbContextFactory).Assembly.FullName));
            return new AccountDbContext(optionsBuilder.Options);
        }
    }
}
