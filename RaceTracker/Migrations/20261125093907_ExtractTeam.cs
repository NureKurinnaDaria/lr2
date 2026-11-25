using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RaceTracker.Migrations
{
    /// <inheritdoc />
    public partial class ExtractTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_Teams", x => x.TeamId));

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Name",
                table: "Teams",
                column: "Name",
                unique: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Athletes",
                type: "integer",
                nullable: true);

            // перенесення даних
            migrationBuilder.Sql(
                @"INSERT INTO ""Teams"" (""Name"") SELECT DISTINCT ""Team"" FROM ""Athletes"" WHERE ""Team"" IS NOT NULL;");
            migrationBuilder.Sql(
                @"UPDATE ""Athletes"" a SET ""TeamId"" = t.""TeamId""
                  FROM ""Teams"" t WHERE t.""Name"" = a.""Team"";");

            migrationBuilder.CreateIndex(
                name: "IX_Athletes_TeamId",
                table: "Athletes",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Athletes_Teams_TeamId",
                table: "Athletes",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.DropColumn(
                name: "Team",
                table: "Athletes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Team",
                table: "Athletes",
                type: "text",
                nullable: true);

            migrationBuilder.Sql(
                @"UPDATE ""Athletes"" a SET ""Team"" = t.""Name""
                  FROM ""Teams"" t WHERE t.""TeamId"" = a.""TeamId"";");

            migrationBuilder.DropForeignKey(
                name: "FK_Athletes_Teams_TeamId",
                table: "Athletes");

            migrationBuilder.DropIndex(
                name: "IX_Athletes_TeamId",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Athletes");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}