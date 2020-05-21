using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fund.API.Infrastructure.Migrations
{
    public partial class UpdateTableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_catalog_catalogbrand_FundBrandId",
                table: "catalog");

            migrationBuilder.DropForeignKey(
                name: "FK_catalog_FundTypes_FundTypeId",
                table: "catalog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FundTypes",
                table: "FundTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_catalog",
                table: "catalog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_catalogbrand",
                table: "catalogbrand");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FundType",
                table: "FundTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fund",
                table: "catalog",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FundBrand",
                table: "catalogbrand",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fund_FundBrand_FundBrandId",
                table: "catalog",
                column: "FundBrandId",
                principalTable: "catalogbrand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fund_FundType_FundTypeId",
                table: "catalog",
                column: "FundTypeId",
                principalTable: "FundTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.RenameIndex(
                name: "IX_catalog_FundTypeId",
                table: "catalog",
                newName: "IX_Fund_FundTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_catalog_FundBrandId",
                table: "catalog",
                newName: "IX_Fund_FundBrandId");

            migrationBuilder.RenameTable(
                name: "FundTypes",
                newName: "FundType");

            migrationBuilder.RenameTable(
                name: "catalog",
                newName: "Fund");

            migrationBuilder.RenameTable(
                name: "catalogbrand",
                newName: "FundBrand");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fund_FundBrand_FundBrandId",
                table: "Fund");

            migrationBuilder.DropForeignKey(
                name: "FK_Fund_FundType_FundTypeId",
                table: "Fund");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FundType",
                table: "FundType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fund",
                table: "Fund");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FundBrand",
                table: "FundBrand");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FundTypes",
                table: "FundType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_catalog",
                table: "Fund",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_catalogbrand",
                table: "FundBrand",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_catalog_catalogbrand_FundBrandId",
                table: "Fund",
                column: "FundBrandId",
                principalTable: "FundBrand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_catalog_FundTypes_FundTypeId",
                table: "Fund",
                column: "FundTypeId",
                principalTable: "FundType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.RenameIndex(
                name: "IX_Fund_FundTypeId",
                table: "Fund",
                newName: "IX_catalog_FundTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Fund_FundBrandId",
                table: "Fund",
                newName: "IX_catalog_FundBrandId");

            migrationBuilder.RenameTable(
                name: "FundType",
                newName: "FundTypes");

            migrationBuilder.RenameTable(
                name: "Fund",
                newName: "catalog");

            migrationBuilder.RenameTable(
                name: "FundBrand",
                newName: "catalogbrand");
        }
    }
}
