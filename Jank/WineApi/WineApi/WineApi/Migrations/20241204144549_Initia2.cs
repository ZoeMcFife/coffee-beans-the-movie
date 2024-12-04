using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WineApi.Migrations
{
    /// <inheritdoc />
    public partial class Initia2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wines_MostTreatments_MostTreatmentId",
                table: "Wines");

            migrationBuilder.AlterColumn<int>(
                name: "MostTreatmentId",
                table: "Wines",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Wines_MostTreatments_MostTreatmentId",
                table: "Wines",
                column: "MostTreatmentId",
                principalTable: "MostTreatments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wines_MostTreatments_MostTreatmentId",
                table: "Wines");

            migrationBuilder.AlterColumn<int>(
                name: "MostTreatmentId",
                table: "Wines",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Wines_MostTreatments_MostTreatmentId",
                table: "Wines",
                column: "MostTreatmentId",
                principalTable: "MostTreatments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
