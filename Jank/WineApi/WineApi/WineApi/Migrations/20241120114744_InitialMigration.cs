using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WineApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MostWeight = table.Column<float>(type: "real", nullable: false),
                    HarvestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VolumeInHectoLitre = table.Column<float>(type: "real", nullable: false),
                    Container = table.Column<string>(type: "text", nullable: false),
                    ProductionType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Additives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Time = table.Column<string>(type: "text", nullable: false),
                    AmountGrammsPerLitre = table.Column<float>(type: "real", nullable: false),
                    AmountGrammsPerHectoLitre = table.Column<float>(type: "real", nullable: false),
                    AmountGrammsPer1000Litre = table.Column<float>(type: "real", nullable: false),
                    WineId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Additives", x => x.Id);
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Density = table.Column<float>(type: "real", nullable: false),
                    WineId = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "MostTreatments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WineId = table.Column<int>(type: "integer", nullable: false),
                    IsTreated = table.Column<bool>(type: "boolean", nullable: false),
                    TreatmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MostTreatments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MostTreatments_Wines_WineId",
                        column: x => x.WineId,
                        principalTable: "Wines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Additives_WineId",
                table: "Additives",
                column: "WineId");

            migrationBuilder.CreateIndex(
                name: "IX_FermentationEntries_WineId",
                table: "FermentationEntries",
                column: "WineId");

            migrationBuilder.CreateIndex(
                name: "IX_MostTreatments_WineId",
                table: "MostTreatments",
                column: "WineId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Additives");

            migrationBuilder.DropTable(
                name: "FermentationEntries");

            migrationBuilder.DropTable(
                name: "MostTreatments");

            migrationBuilder.DropTable(
                name: "Wines");
        }
    }
}
