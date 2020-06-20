using Microsoft.EntityFrameworkCore.Migrations;

namespace ShareOwner.API.Infrastructure.ShareOwnerMigrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_ShareOwner_ShareOwnerId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_ShareOwnerId",
                table: "Reservation");
        }
    }
}
