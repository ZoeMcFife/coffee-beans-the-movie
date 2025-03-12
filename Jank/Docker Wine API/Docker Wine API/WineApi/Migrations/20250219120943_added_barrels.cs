using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WineApi.Migrations
{
    /// <inheritdoc />
    public partial class added_barrels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Additives_Wines_WineId",
                table: "Additives");

            migrationBuilder.DropForeignKey(
                name: "FK_FermentationEntries_Wines_WineId",
                table: "FermentationEntries");

            migrationBuilder.DropTable(
                name: "Wines");

            migrationBuilder.DropIndex(
                name: "IX_FermentationEntries_WineId",
                table: "FermentationEntries");

            migrationBuilder.DropIndex(
                name: "IX_Additives_WineId",
                table: "Additives");

            migrationBuilder.AddColumn<bool>(
                name: "AdminRights",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "WineBarrelId",
                table: "FermentationEntries",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WineBarrelId",
                table: "Additives",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WineTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WineTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WineBarrels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentWineTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    CurrentWineBarrelHistoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MostWeight = table.Column<float>(type: "real", nullable: false),
                    HarvestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VolumeInLitre = table.Column<float>(type: "real", nullable: false),
                    Container = table.Column<string>(type: "text", nullable: false),
                    ProductionType = table.Column<string>(type: "text", nullable: false),
                    MostTreatmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    WineTypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WineBarrels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WineBarrels_MostTreatments_MostTreatmentId",
                        column: x => x.MostTreatmentId,
                        principalTable: "MostTreatments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WineBarrels_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WineBarrels_WineTypes_WineTypeId",
                        column: x => x.WineTypeId,
                        principalTable: "WineTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WineBarrelHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WineBarrelId = table.Column<Guid>(type: "uuid", nullable: false),
                    WineTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WineBarrelHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WineBarrelHistories_WineBarrels_WineBarrelId",
                        column: x => x.WineBarrelId,
                        principalTable: "WineBarrels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WineBarrelHistories_WineTypes_WineTypeId",
                        column: x => x.WineTypeId,
                        principalTable: "WineTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FermentationEntries_WineBarrelId",
                table: "FermentationEntries",
                column: "WineBarrelId");

            migrationBuilder.CreateIndex(
                name: "IX_Additives_WineBarrelId",
                table: "Additives",
                column: "WineBarrelId");

            migrationBuilder.CreateIndex(
                name: "IX_WineBarrelHistories_WineBarrelId",
                table: "WineBarrelHistories",
                column: "WineBarrelId");

            migrationBuilder.CreateIndex(
                name: "IX_WineBarrelHistories_WineTypeId",
                table: "WineBarrelHistories",
                column: "WineTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WineBarrels_MostTreatmentId",
                table: "WineBarrels",
                column: "MostTreatmentId");

            migrationBuilder.CreateIndex(
                name: "IX_WineBarrels_UserId",
                table: "WineBarrels",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WineBarrels_WineTypeId",
                table: "WineBarrels",
                column: "WineTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Additives_WineBarrels_WineBarrelId",
                table: "Additives",
                column: "WineBarrelId",
                principalTable: "WineBarrels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FermentationEntries_WineBarrels_WineBarrelId",
                table: "FermentationEntries",
                column: "WineBarrelId",
                principalTable: "WineBarrels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Additives_WineBarrels_WineBarrelId",
                table: "Additives");

            migrationBuilder.DropForeignKey(
                name: "FK_FermentationEntries_WineBarrels_WineBarrelId",
                table: "FermentationEntries");

            migrationBuilder.DropTable(
                name: "WineBarrelHistories");

            migrationBuilder.DropTable(
                name: "WineBarrels");

            migrationBuilder.DropTable(
                name: "WineTypes");

            migrationBuilder.DropIndex(
                name: "IX_FermentationEntries_WineBarrelId",
                table: "FermentationEntries");

            migrationBuilder.DropIndex(
                name: "IX_Additives_WineBarrelId",
                table: "Additives");

            migrationBuilder.DropColumn(
                name: "AdminRights",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WineBarrelId",
                table: "FermentationEntries");

            migrationBuilder.DropColumn(
                name: "WineBarrelId",
                table: "Additives");

            migrationBuilder.CreateTable(
                name: "Wines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MostTreatmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Container = table.Column<string>(type: "text", nullable: false),
                    HarvestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MostWeight = table.Column<float>(type: "real", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProductionType = table.Column<string>(type: "text", nullable: false),
                    VolumeInHectoLitre = table.Column<float>(type: "real", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_FermentationEntries_WineId",
                table: "FermentationEntries",
                column: "WineId");

            migrationBuilder.CreateIndex(
                name: "IX_Additives_WineId",
                table: "Additives",
                column: "WineId");

            migrationBuilder.CreateIndex(
                name: "IX_Wines_MostTreatmentId",
                table: "Wines",
                column: "MostTreatmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Wines_UserId",
                table: "Wines",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Additives_Wines_WineId",
                table: "Additives",
                column: "WineId",
                principalTable: "Wines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FermentationEntries_Wines_WineId",
                table: "FermentationEntries",
                column: "WineId",
                principalTable: "Wines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
