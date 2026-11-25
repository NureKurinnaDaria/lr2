using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddRaceStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Races",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Scheduled");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Races");
        }
    }
}
