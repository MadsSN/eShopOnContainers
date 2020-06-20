using Microsoft.EntityFrameworkCore.Migrations;

namespace ShareOwner.API.Infrastructure.ShareOwnerMigrations
{
    public partial class ReservationStatus2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Reservation",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Reservation",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
