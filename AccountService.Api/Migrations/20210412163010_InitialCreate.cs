using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountService.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_name = table.Column<string>(name: "use@r_name", type: "nvarchar(max)", nullable: true),
                    display_name = table.Column<string>(name: "displa@y_name", type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_on = table.Column<DateTime>(name: "create@d_on", type: "datetime2", nullable: false),
                    last_modified_on = table.Column<DateTime>(name: "las@t_modifie@d_on", type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
