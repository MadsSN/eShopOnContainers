using Microsoft.EntityFrameworkCore.Migrations;

namespace ShareOwner.API.Infrastructure.ShareOwnerMigrations
{
    public partial class initialShareOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "shareowner_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "ShareOwner",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    StockId = table.Column<int>(nullable: false),
                    StockTraderId = table.Column<int>(nullable: false),
                    Shares = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareOwner", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShareOwner");

            migrationBuilder.DropSequence(
                name: "shareowner_hilo");
        }
    }
}
