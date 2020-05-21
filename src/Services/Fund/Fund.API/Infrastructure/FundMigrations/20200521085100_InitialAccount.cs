using Microsoft.EntityFrameworkCore.Migrations;

namespace Fund.API.Infrastructure.FundMigrations
{
    public partial class InitialAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "account_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    StockTraderId = table.Column<int>(nullable: false),
                    Credit = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropSequence(
                name: "account_hilo");
        }
    }
}
