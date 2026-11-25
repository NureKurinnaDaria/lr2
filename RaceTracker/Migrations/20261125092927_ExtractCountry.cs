using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RaceTracker.Migrations
{
    /// <inheritdoc />
    public partial class ExtractCountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_Countries", x => x.CountryId));

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Code",
                table: "Countries",
                column: "Code",
                unique: true);

            // 1) нова колонка спочатку nullable
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Athletes",
                type: "integer",
                nullable: true);

            // 2) перенесення даних: унікальні країни та зв'язок
            migrationBuilder.Sql(
                @"INSERT INTO ""Countries"" (""Code"") SELECT DISTINCT ""Country"" FROM ""Athletes"";");
            migrationBuilder.Sql(
                @"UPDATE ""Athletes"" a SET ""CountryId"" = c.""CountryId""
                  FROM ""Countries"" c WHERE c.""Code"" = a.""Country"";");

            // 3) обмеження, ключ, видалення старої колонки
            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Athletes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Athletes_CountryId",
                table: "Athletes",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Athletes_Countries_CountryId",
                table: "Athletes",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Athletes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Athletes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(
                @"UPDATE ""Athletes"" a SET ""Country"" = c.""Code""
                  FROM ""Countries"" c WHERE c.""CountryId"" = a.""CountryId"";");

            migrationBuilder.DropForeignKey(
                name: "FK_Athletes_Countries_CountryId",
                table: "Athletes");

            migrationBuilder.DropIndex(
                name: "IX_Athletes_CountryId",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Athletes");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}