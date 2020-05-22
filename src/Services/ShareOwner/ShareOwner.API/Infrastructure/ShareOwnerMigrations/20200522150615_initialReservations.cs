using Microsoft.EntityFrameworkCore.Migrations;

namespace ShareOwner.API.Infrastructure.ShareOwnerMigrations
{
    public partial class initialReservations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ShareOwnerId = table.Column<int>(nullable: false),
                    Reserved = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservation_ShareOwner_ShareOwnerId",
                        column: x => x.ShareOwnerId,
                        principalTable: "ShareOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ShareOwnerId",
                table: "Reservation",
                column: "ShareOwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservation");
        }
    }
}
