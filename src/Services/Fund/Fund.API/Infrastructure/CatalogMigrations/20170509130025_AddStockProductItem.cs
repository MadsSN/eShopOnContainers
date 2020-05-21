using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fund.API.Infrastructure.Migrations
{
    public partial class AddStockProductItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableStock",
                table: "Fund",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxStockThreshold",
                table: "Fund",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "OnReorder",
                table: "Fund",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RestockThreshold",
                table: "Fund",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableStock",
                table: "Fund");

            migrationBuilder.DropColumn(
                name: "MaxStockThreshold",
                table: "Fund");

            migrationBuilder.DropColumn(
                name: "OnReorder",
                table: "Fund");

            migrationBuilder.DropColumn(
                name: "RestockThreshold",
                table: "Fund");
        }
    }
}
