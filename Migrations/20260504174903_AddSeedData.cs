using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HandballCompetitionManager.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "CreatedAt", "DisplayName", "Email", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin User", "admin@handball.local", 1, "admin" },
                    { 2, new DateTime(2025, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Coach Horvat", "coach1@handball.local", 3, "coach_horvat" },
                    { 3, new DateTime(2025, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Coach Vukovic", "coach2@handball.local", 3, "coach_vukovic" },
                    { 4, new DateTime(2025, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest User", "guest@handball.local", 4, "guest_user" }
                });

            migrationBuilder.InsertData(
                table: "Competitions",
                columns: new[] { "Id", "City", "EndDate", "Name", "Season", "StartDate" },
                values: new object[,]
                {
                    { 1, "Zagreb", new DateTime(2025, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Croatian League 2025", "2024/2025", new DateTime(2025, 5, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Split", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Croatian Cup 2025", "2024/2025", new DateTime(2025, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Zagreb", new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Regional Championship", "2025", new DateTime(2025, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "Club", "CoachName", "FoundedYear", "HomeArena", "HomeCity", "Name" },
                values: new object[,]
                {
                    { 1, "Zagreb HC", "Coach Horvat", 2005, "Arena Zagreb", "Zagreb", "Zagreb Handball" },
                    { 2, "Split HC", "Coach Vukovic", 2008, "Gradski Vrt", "Split", "Split Warriors" },
                    { 3, "Rijeka HC", "Coach Ivanovic", 2003, "Mladost Arena", "Rijeka", "Rijeka Sharks" },
                    { 4, "Osijek HC", "Coach Grgic", 2010, "Gradski Vrt Osijek", "Osijek", "Osijek Titans" },
                    { 5, "Zadar HC", "Coach Milic", 2006, "Zadar Arena", "Zadar", "Zadar Eagles" }
                });

            migrationBuilder.InsertData(
                table: "GroupPhases",
                columns: new[] { "Id", "CompetitionId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Group A" },
                    { 2, 1, "Group B" },
                    { 3, 2, "Finals" }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Assists", "BirthDate", "FirstName", "GoalsScored", "JerseyNumber", "LastName", "Position", "TeamId" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(1990, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marko", 0, 1, "Horvat", 1, 1 },
                    { 2, 12, new DateTime(1995, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ivan", 45, 7, "Novak", 2, 1 },
                    { 3, 8, new DateTime(1993, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Petar", 52, 8, "Milic", 6, 1 },
                    { 4, 15, new DateTime(1992, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Damir", 38, 10, "Kitic", 7, 1 },
                    { 5, 0, new DateTime(1988, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ante", 0, 1, "Vukovic", 1, 2 },
                    { 6, 10, new DateTime(1996, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Luka", 48, 9, "Bevandic", 6, 2 },
                    { 7, 20, new DateTime(1994, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Stipe", 35, 11, "Mandic", 7, 2 },
                    { 8, 0, new DateTime(1989, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Goran", 0, 1, "Ivanovic", 1, 3 },
                    { 9, 11, new DateTime(1997, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nikola", 42, 5, "Salic", 2, 3 },
                    { 10, 18, new DateTime(1991, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dino", 40, 12, "Grgic", 7, 4 },
                    { 11, 9, new DateTime(1994, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zoran", 46, 13, "Milic", 6, 5 }
                });

            migrationBuilder.InsertData(
                table: "Matches",
                columns: new[] { "Id", "AwayScore", "AwayTeamId", "CompetitionId", "GroupId", "HomeScore", "HomeTeamId", "Kickoff", "MaintenanceHall", "RoundNumber", "Status" },
                values: new object[,]
                {
                    { 1, 25, 2, 1, 1, 28, 1, new DateTime(2025, 5, 5, 19, 0, 0, 0, DateTimeKind.Unspecified), "", 0, 1 },
                    { 2, 0, 4, 1, 1, 0, 3, new DateTime(2025, 5, 6, 19, 0, 0, 0, DateTimeKind.Unspecified), "", 0, 1 },
                    { 3, 0, 1, 1, 2, 0, 5, new DateTime(2025, 5, 7, 19, 0, 0, 0, DateTimeKind.Unspecified), "", 0, 1 },
                    { 4, 29, 3, 2, 3, 31, 2, new DateTime(2025, 5, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), "", 0, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Competitions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "GroupPhases",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GroupPhases",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GroupPhases",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Competitions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Competitions",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
