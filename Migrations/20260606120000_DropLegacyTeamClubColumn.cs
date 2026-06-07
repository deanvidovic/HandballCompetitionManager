using HandballCompetitionManager.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandballCompetitionManager.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(HandballDbContext))]
    [Migration("20260606120000_DropLegacyTeamClubColumn")]
    public partial class DropLegacyTeamClubColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF COL_LENGTH('Teams', 'Club') IS NOT NULL
                BEGIN
                    ALTER TABLE [Teams] DROP COLUMN [Club];
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF COL_LENGTH('Teams', 'Club') IS NULL
                BEGIN
                    ALTER TABLE [Teams] ADD [Club] nvarchar(max) NOT NULL CONSTRAINT [DF_Teams_Club] DEFAULT N'';
                    ALTER TABLE [Teams] DROP CONSTRAINT [DF_Teams_Club];
                END
                """);
        }
    }
}
