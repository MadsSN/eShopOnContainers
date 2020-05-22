using Microsoft.EntityFrameworkCore.Migrations;

namespace Stock.API.Infrastructure.StockMigrations
{
    public partial class InitialStockName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stock",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "Stock",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
