using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fund.API.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "catalog_brand_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "catalog_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "catalog_type_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "catalogbrand",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Brand = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalogbrand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FundTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Type = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "catalog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    FundBrandId = table.Column<int>(nullable: false),
                    FundTypeId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    PictureUri = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_catalog_catalogbrand_FundBrandId",
                        column: x => x.FundBrandId,
                        principalTable: "catalogbrand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_catalog_FundTypes_FundTypeId",
                        column: x => x.FundTypeId,
                        principalTable: "FundTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_catalog_FundBrandId",
                table: "catalog",
                column: "FundBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_catalog_FundTypeId",
                table: "catalog",
                column: "FundTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "catalog_brand_hilo");

            migrationBuilder.DropSequence(
                name: "catalog_hilo");

            migrationBuilder.DropSequence(
                name: "catalog_type_hilo");

            migrationBuilder.DropTable(
                name: "catalog");

            migrationBuilder.DropTable(
                name: "catalogbrand");

            migrationBuilder.DropTable(
                name: "FundTypes");
        }
    }
}
