using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AviTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixEmailTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Emial",
                table: "Users",
                newName: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "Emial");
        }
    }
}
