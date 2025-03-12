using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WineApi.Migrations
{
    /// <inheritdoc />
    public partial class remmovedsmth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WineBarrels_WineTypes_WineTypeId",
                table: "WineBarrels");

            migrationBuilder.DropIndex(
                name: "IX_WineBarrels_WineTypeId",
                table: "WineBarrels");

            migrationBuilder.DropColumn(
                name: "WineTypeId",
                table: "WineBarrels");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WineTypeId",
                table: "WineBarrels",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WineBarrels_WineTypeId",
                table: "WineBarrels",
                column: "WineTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_WineBarrels_WineTypes_WineTypeId",
                table: "WineBarrels",
                column: "WineTypeId",
                principalTable: "WineTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
