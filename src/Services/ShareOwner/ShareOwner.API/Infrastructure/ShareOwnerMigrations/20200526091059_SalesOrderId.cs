using Microsoft.EntityFrameworkCore.Migrations;

namespace ShareOwner.API.Infrastructure.ShareOwnerMigrations
{
    public partial class SalesOrderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SalesOrderId",
                table: "Reservation",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalesOrderId",
                table: "Reservation");
        }
    }
}
