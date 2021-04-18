using AccountService.Domain.Models;
using Foudation.CQRS.Data;
using Foundation.CQRS.Core;
using Microsoft.EntityFrameworkCore;


namespace AccountService.Domain
{
    public class AccountDbContext:AbStractDbContext
    {
        public AccountDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            ConvertToSnakeCase(builder);
        }
        private void ConvertToSnakeCase(ModelBuilder builder) {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().ToSnakeCase());
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToSnakeCase());
                }
                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToSnakeCase());
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.SetName(index.GetName().ToSnakeCase());
                }
            }
        }
        public DbSet<Users> Users { get; set; }
    }

}
