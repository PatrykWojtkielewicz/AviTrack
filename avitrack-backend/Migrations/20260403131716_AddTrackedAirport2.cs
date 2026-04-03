using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AviTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTrackedAirport2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackedAirports_Users_USerId",
                table: "TrackedAirports");

            migrationBuilder.RenameColumn(
                name: "USerId",
                table: "TrackedAirports",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TrackedAirports_USerId",
                table: "TrackedAirports",
                newName: "IX_TrackedAirports_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedAirports_Users_UserId",
                table: "TrackedAirports",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackedAirports_Users_UserId",
                table: "TrackedAirports");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TrackedAirports",
                newName: "USerId");

            migrationBuilder.RenameIndex(
                name: "IX_TrackedAirports_UserId",
                table: "TrackedAirports",
                newName: "IX_TrackedAirports_USerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedAirports_Users_USerId",
                table: "TrackedAirports",
                column: "USerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
