using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AccountManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class Final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TrainStations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainStations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfTrains",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfTrains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trains",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Line = table.Column<int>(type: "int", nullable: false),
                    AmountOfPassagers = table.Column<int>(type: "int", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: true),
                    TypeOfTrainId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trains_TypeOfTrains_TypeOfTrainId",
                        column: x => x.TypeOfTrainId,
                        principalTable: "TypeOfTrains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromWhere = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToWhere = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartsAtStation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivesAtDestination = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrainId = table.Column<int>(type: "int", nullable: false),
                    TrainStationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_TrainStations_TrainStationId",
                        column: x => x.TrainStationId,
                        principalTable: "TrainStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedules_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TypeOfTrains",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Скоростен" },
                    { 2, "Пътнически" },
                    { 3, "Градски" },
                    { 4, "Извънградски" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TrainId",
                table: "Schedules",
                column: "TrainId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TrainStationId",
                table: "Schedules",
                column: "TrainStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Trains_TypeOfTrainId",
                table: "Trains",
                column: "TypeOfTrainId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "TrainStations");

            migrationBuilder.DropTable(
                name: "Trains");

            migrationBuilder.DropTable(
                name: "TypeOfTrains");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
