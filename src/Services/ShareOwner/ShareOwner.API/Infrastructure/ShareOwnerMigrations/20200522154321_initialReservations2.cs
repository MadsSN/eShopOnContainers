using Microsoft.EntityFrameworkCore.Migrations;

namespace ShareOwner.API.Infrastructure.ShareOwnerMigrations
{
    public partial class initialReservations2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_ShareOwner_ShareOwnerId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_ShareOwnerId",
                table: "Reservation");

            migrationBuilder.CreateSequence(
                name: "reservation_hilo",
                incrementBy: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "reservation_hilo");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ShareOwnerId",
                table: "Reservation",
                column: "ShareOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_ShareOwner_ShareOwnerId",
                table: "Reservation",
                column: "ShareOwnerId",
                principalTable: "ShareOwner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
