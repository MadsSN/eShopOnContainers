using Microsoft.EntityFrameworkCore.Migrations;

namespace Order.API.Infrastructure.OrderMigrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "buyorder_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "saleorder_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "BuyOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    StockTraderId = table.Column<int>(nullable: false),
                    StockId = table.Column<int>(nullable: false),
                    SharesCount = table.Column<decimal>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    PricePerShare = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    StockTraderId = table.Column<int>(nullable: false),
                    StockId = table.Column<int>(nullable: false),
                    SharesCount = table.Column<decimal>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    StockId = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.StockId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyOrder");

            migrationBuilder.DropTable(
                name: "SalesOrder");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropSequence(
                name: "buyorder_hilo");

            migrationBuilder.DropSequence(
                name: "saleorder_hilo");
        }
    }
}
