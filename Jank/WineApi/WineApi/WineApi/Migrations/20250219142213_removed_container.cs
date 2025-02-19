using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WineApi.Migrations
{
    /// <inheritdoc />
    public partial class removed_container : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Container",
                table: "WineBarrels");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WineBarrels",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WineBarrels",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Container",
                table: "WineBarrels",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
