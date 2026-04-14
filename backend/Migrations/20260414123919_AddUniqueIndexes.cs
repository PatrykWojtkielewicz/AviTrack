using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AviTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrackedFlights_UserId",
                table: "TrackedFlights");

            migrationBuilder.DropIndex(
                name: "IX_TrackedAirports_UserId",
                table: "TrackedAirports");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackedFlights_UserId_Callsign",
                table: "TrackedFlights",
                columns: new[] { "UserId", "Callsign" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackedAirports_UserId_IcaoCode",
                table: "TrackedAirports",
                columns: new[] { "UserId", "IcaoCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TrackedFlights_UserId_Callsign",
                table: "TrackedFlights");

            migrationBuilder.DropIndex(
                name: "IX_TrackedAirports_UserId_IcaoCode",
                table: "TrackedAirports");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedFlights_UserId",
                table: "TrackedFlights",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedAirports_UserId",
                table: "TrackedAirports",
                column: "UserId");
        }
    }
}
