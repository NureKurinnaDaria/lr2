using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddResultIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Results_AthleteId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_RaceId",
                table: "Results");

            migrationBuilder.CreateIndex(
                name: "IX_Results_AthleteId_RaceId",
                table: "Results",
                columns: new[] { "AthleteId", "RaceId" });

            migrationBuilder.CreateIndex(
                name: "IX_Results_RaceId_CheckpointNo",
                table: "Results",
                columns: new[] { "RaceId", "CheckpointNo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Results_AthleteId_RaceId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_RaceId_CheckpointNo",
                table: "Results");

            migrationBuilder.CreateIndex(
                name: "IX_Results_AthleteId",
                table: "Results",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_RaceId",
                table: "Results",
                column: "RaceId");
        }
    }
}
