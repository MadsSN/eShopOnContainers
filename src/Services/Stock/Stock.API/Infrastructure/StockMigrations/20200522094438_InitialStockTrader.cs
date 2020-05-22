using Microsoft.EntityFrameworkCore.Migrations;

namespace Stock.API.Infrastructure.StockMigrations
{
    public partial class InitialStockTrader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "stock_trader_hilo",
                incrementBy: 10);

            migrationBuilder.AddColumn<int>(
                name: "TotalShares",
                table: "Stock",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "StockTrader",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    StockTraderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTrader", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockTrader");

            migrationBuilder.DropSequence(
                name: "stock_trader_hilo");

            migrationBuilder.DropColumn(
                name: "TotalShares",
                table: "Stock");
        }
    }
}
