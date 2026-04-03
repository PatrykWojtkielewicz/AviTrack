using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AviTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTrackedAirport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrackedAirports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    USerId = table.Column<int>(type: "INTEGER", nullable: false),
                    IcaoCode = table.Column<string>(type: "TEXT", nullable: false),
                    CustomLabel = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedAirports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackedAirports_Users_USerId",
                        column: x => x.USerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackedAirports_USerId",
                table: "TrackedAirports",
                column: "USerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackedAirports");
        }
    }
}
