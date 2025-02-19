using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WineApi.Migrations
{
    /// <inheritdoc />
    public partial class added_guids : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditiveTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditiveTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MostTreatments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsTreated = table.Column<bool>(type: "boolean", nullable: false),
                    TreatmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MostTreatments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MostWeight = table.Column<float>(type: "real", nullable: false),
                    HarvestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VolumeInHectoLitre = table.Column<float>(type: "real", nullable: false),
                    Container = table.Column<string>(type: "text", nullable: false),
                    ProductionType = table.Column<string>(type: "text", nullable: false),
                    MostTreatmentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wines_MostTreatments_MostTreatmentId",
                        column: x => x.MostTreatmentId,
                        principalTable: "MostTreatments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Wines_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Additives",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AmountGrammsPerLitre = table.Column<float>(type: "real", nullable: false),
                    AdditiveTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    WineId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Additives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Additives_AdditiveTypes_AdditiveTypeId",
                        column: x => x.AdditiveTypeId,
                        principalTable: "AdditiveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Additives_Wines_WineId",
                        column: x => x.WineId,
                        principalTable: "Wines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FermentationEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Density = table.Column<float>(type: "real", nullable: false),
                    WineId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FermentationEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FermentationEntries_Wines_WineId",
                        column: x => x.WineId,
                        principalTable: "Wines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Additives_AdditiveTypeId",
                table: "Additives",
                column: "AdditiveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Additives_WineId",
                table: "Additives",
                column: "WineId");

            migrationBuilder.CreateIndex(
                name: "IX_FermentationEntries_WineId",
                table: "FermentationEntries",
                column: "WineId");

            migrationBuilder.CreateIndex(
                name: "IX_Wines_MostTreatmentId",
                table: "Wines",
                column: "MostTreatmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Wines_UserId",
                table: "Wines",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Additives");

            migrationBuilder.DropTable(
                name: "FermentationEntries");

            migrationBuilder.DropTable(
                name: "AdditiveTypes");

            migrationBuilder.DropTable(
                name: "Wines");

            migrationBuilder.DropTable(
                name: "MostTreatments");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
