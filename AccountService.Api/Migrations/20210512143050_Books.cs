using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountService.Api.Migrations
{
    public partial class Books : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(name: "created_on", nullable: false),
                    last_modified_on = table.Column<DateTime>(name: "last_modified_on", nullable: false),
                    book_name = table.Column<string>(name: "book_name", nullable: true),
                    published = table.Column<string>(nullable: true),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    auther = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_books", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "books");
        }
    }
}
