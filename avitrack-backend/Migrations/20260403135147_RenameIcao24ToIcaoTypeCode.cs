using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AviTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class RenameIcao24ToIcaoTypeCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Icao24",
                table: "TrackedAircraftTypes",
                newName: "IcaoTypeCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IcaoTypeCode",
                table: "TrackedAircraftTypes",
                newName: "Icao24");
        }
    }
}
