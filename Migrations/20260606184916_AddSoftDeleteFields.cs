using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandballCompetitionManager.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF COL_LENGTH('Teams', 'DeletedAt') IS NULL
                    ALTER TABLE [Teams] ADD [DeletedAt] datetime2 NULL;
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH('Players', 'DeletedAt') IS NULL
                    ALTER TABLE [Players] ADD [DeletedAt] datetime2 NULL;
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH('Competitions', 'DeletedAt') IS NULL
                    ALTER TABLE [Competitions] ADD [DeletedAt] datetime2 NULL;
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH('AppUsers', 'DeletedAt') IS NULL
                    ALTER TABLE [AppUsers] ADD [DeletedAt] datetime2 NULL;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF COL_LENGTH('Teams', 'DeletedAt') IS NOT NULL
                    ALTER TABLE [Teams] DROP COLUMN [DeletedAt];
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH('Players', 'DeletedAt') IS NOT NULL
                    ALTER TABLE [Players] DROP COLUMN [DeletedAt];
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH('Competitions', 'DeletedAt') IS NOT NULL
                    ALTER TABLE [Competitions] DROP COLUMN [DeletedAt];
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH('AppUsers', 'DeletedAt') IS NOT NULL
                    ALTER TABLE [AppUsers] DROP COLUMN [DeletedAt];
                """);
        }
    }
}
