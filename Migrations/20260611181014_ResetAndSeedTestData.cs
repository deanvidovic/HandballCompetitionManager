using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandballCompetitionManager.Migrations
{
    /// <inheritdoc />
    public partial class ResetAndSeedTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[MatchAttachments]', N'U') IS NOT NULL
                    DELETE FROM [MatchAttachments];

                DELETE FROM [Matches];
                DELETE FROM [GroupPhaseTeams];
                DELETE FROM [TeamCompetitions];
                DELETE FROM [CompetitionAdministrators];
                DELETE FROM [Players];
                DELETE FROM [GroupPhases];
                DELETE FROM [Competitions];
                DELETE FROM [Teams];

                DELETE ur
                FROM [AspNetUserRoles] ur
                INNER JOIN [AppUsers] u ON u.[Id] = ur.[UserId]
                WHERE u.[NormalizedEmail] IN (
                    N'SUPERADMIN@TEST.COM',
                    N'COMPETITION.MANAGER1@TEST.COM',
                    N'COMPETITION.MANAGER2@TEST.COM',
                    N'COMPETITION.MANAGER3@TEST.COM',
                    N'COMPETITION.MANAGER4@TEST.COM',
                    N'ADMIN@HANDBALL.LOCAL',
                    N'COACH1@HANDBALL.LOCAL',
                    N'COACH2@HANDBALL.LOCAL',
                    N'GUEST@HANDBALL.LOCAL',
                    N'VIDOVICDEAN@GMAIL.COM',
                    N'MANAGER@HANDBALL.LOCAL',
                    N'COACH.HORVAT@HANDBALL.LOCAL',
                    N'COACH.VUKOVIC@HANDBALL.LOCAL'
                );

                DELETE FROM [AppUsers]
                WHERE [NormalizedEmail] IN (
                    N'SUPERADMIN@TEST.COM',
                    N'COMPETITION.MANAGER1@TEST.COM',
                    N'COMPETITION.MANAGER2@TEST.COM',
                    N'COMPETITION.MANAGER3@TEST.COM',
                    N'COMPETITION.MANAGER4@TEST.COM',
                    N'ADMIN@HANDBALL.LOCAL',
                    N'COACH1@HANDBALL.LOCAL',
                    N'COACH2@HANDBALL.LOCAL',
                    N'GUEST@HANDBALL.LOCAL',
                    N'VIDOVICDEAN@GMAIL.COM',
                    N'MANAGER@HANDBALL.LOCAL',
                    N'COACH.HORVAT@HANDBALL.LOCAL',
                    N'COACH.VUKOVIC@HANDBALL.LOCAL'
                );

                IF NOT EXISTS (SELECT 1 FROM [AspNetRoles] WHERE [NormalizedName] = N'ADMIN')
                BEGIN
                    SET IDENTITY_INSERT [AspNetRoles] ON;
                    INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
                    VALUES (1001, N'Admin', N'ADMIN', N'test-role-admin');
                    SET IDENTITY_INSERT [AspNetRoles] OFF;
                END;

                IF NOT EXISTS (SELECT 1 FROM [AspNetRoles] WHERE [NormalizedName] = N'MANAGER')
                BEGIN
                    SET IDENTITY_INSERT [AspNetRoles] ON;
                    INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
                    VALUES (1002, N'Manager', N'MANAGER', N'test-role-manager');
                    SET IDENTITY_INSERT [AspNetRoles] OFF;
                END;

                IF NOT EXISTS (SELECT 1 FROM [AspNetRoles] WHERE [NormalizedName] = N'COACH')
                BEGIN
                    SET IDENTITY_INSERT [AspNetRoles] ON;
                    INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
                    VALUES (1003, N'Coach', N'COACH', N'test-role-coach');
                    SET IDENTITY_INSERT [AspNetRoles] OFF;
                END;
                """);

            InsertUsers(migrationBuilder);
            InsertCompetitions(migrationBuilder);
            InsertTeams(migrationBuilder);
            InsertPlayers(migrationBuilder);
            InsertCompetitionTeams(migrationBuilder);
            InsertCompetitionManagers(migrationBuilder);
            InsertUserRoles(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE ur
                FROM [AspNetUserRoles] ur
                WHERE ur.[UserId] BETWEEN 9001 AND 9005;

                DELETE FROM [CompetitionAdministrators]
                WHERE [AdministratorsId] BETWEEN 9001 AND 9005
                   OR [ManagedCompetitionsId] BETWEEN 1001 AND 1004;

                DELETE FROM [TeamCompetitions]
                WHERE [CompetitionsId] BETWEEN 1001 AND 1004
                   OR [TeamsId] BETWEEN 1101 AND 1124;

                DELETE FROM [GroupPhaseTeams]
                WHERE [TeamsId] BETWEEN 1101 AND 1124;

                IF OBJECT_ID(N'[MatchAttachments]', N'U') IS NOT NULL
                BEGIN
                    DELETE ma
                    FROM [MatchAttachments] ma
                    INNER JOIN [Matches] m ON m.[Id] = ma.[MatchId]
                    WHERE m.[Id] BETWEEN 4001 AND 4099;
                END;

                DELETE FROM [Matches] WHERE [Id] BETWEEN 4001 AND 4099;
                DELETE FROM [Players] WHERE [Id] BETWEEN 2001 AND 2336;
                DELETE FROM [GroupPhases] WHERE [Id] BETWEEN 3001 AND 3008;
                DELETE FROM [Teams] WHERE [Id] BETWEEN 1101 AND 1124;
                DELETE FROM [Competitions] WHERE [Id] BETWEEN 1001 AND 1004;
                DELETE FROM [AppUsers] WHERE [Id] BETWEEN 9001 AND 9005;

                DELETE FROM [AspNetRoles]
                WHERE [Id] IN (1001, 1002, 1003)
                  AND [ConcurrencyStamp] IN (N'test-role-admin', N'test-role-manager', N'test-role-coach');
                """);
        }

        private static void InsertUsers(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[]
                {
                    "Id", "Username", "NormalizedUserName", "DisplayName", "Email", "NormalizedEmail",
                    "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "Role",
                    "CreatedAt", "DateOfBirth", "OIB", "JMBG", "AccessFailedCount", "LockoutEnabled",
                    "PhoneNumberConfirmed", "TwoFactorEnabled"
                },
                values: new object[,]
                {
                    { 9001, "superadmin", "SUPERADMIN", "Super Admin", "superadmin@test.com", "SUPERADMIN@TEST.COM", true, "AQAAAAIAAYagAAAAEGCIFjmXLwG7hspuGO8aRyc9ubQoyCcGM5G7VdOrIQmw0xCpD7TIHrqzA04ikz1wGw==", "test-superadmin-security", "test-superadmin-concurrency", 1, new DateTime(2026, 6, 11), new DateTime(1980, 1, 1), "90000000001", "9000000000001", 0, false, false, false },
                    { 9002, "competition.manager1", "COMPETITION.MANAGER1", "Competition Manager 1", "competition.manager1@test.com", "COMPETITION.MANAGER1@TEST.COM", true, "AQAAAAIAAYagAAAAEJbkpKNmcwQ5s0/BRIiqXh2BwJH/i4AW/gXXTb+q3Vl4Q+R4iQpV6uot3DcNIWI7Gg==", "test-manager1-security", "test-manager1-concurrency", 2, new DateTime(2026, 6, 11), new DateTime(1981, 2, 1), "90000000002", "9000000000002", 0, false, false, false },
                    { 9003, "competition.manager2", "COMPETITION.MANAGER2", "Competition Manager 2", "competition.manager2@test.com", "COMPETITION.MANAGER2@TEST.COM", true, "AQAAAAIAAYagAAAAENudi5fb1WX1Z2rzoRtEJgniYW5YEFY6DUk6EUGQpc1r+jCTg3rfrSIJiEX8l1l3og==", "test-manager2-security", "test-manager2-concurrency", 2, new DateTime(2026, 6, 11), new DateTime(1982, 3, 1), "90000000003", "9000000000003", 0, false, false, false },
                    { 9004, "competition.manager3", "COMPETITION.MANAGER3", "Competition Manager 3", "competition.manager3@test.com", "COMPETITION.MANAGER3@TEST.COM", true, "AQAAAAIAAYagAAAAENBxBeZxctuiNLYTjppw5eRjJ3OZ0x2F7xF+CbLLYA6n2C2NCDvGEm10VxTEh8lxvQ==", "test-manager3-security", "test-manager3-concurrency", 2, new DateTime(2026, 6, 11), new DateTime(1983, 4, 1), "90000000004", "9000000000004", 0, false, false, false },
                    { 9005, "competition.manager4", "COMPETITION.MANAGER4", "Competition Manager 4", "competition.manager4@test.com", "COMPETITION.MANAGER4@TEST.COM", true, "AQAAAAIAAYagAAAAEI3iA9lrgi2jHD7Ufx/vhXZTblfw51ohTHbM0RD1V3RjJDWPzUcl3DrHcwyC7v3vnA==", "test-manager4-security", "test-manager4-concurrency", 2, new DateTime(2026, 6, 11), new DateTime(1984, 5, 1), "90000000005", "9000000000005", 0, false, false, false }
                });
        }

        private static void InsertCompetitions(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Competitions",
                columns: new[] { "Id", "Name", "Season", "StartDate", "EndDate", "City" },
                values: new object[,]
                {
                    { 1001, "North Test Championship", "2026/2027", new DateTime(2026, 9, 1), new DateTime(2026, 9, 14), "Zagreb" },
                    { 1002, "Coastal Test Cup", "2026/2027", new DateTime(2026, 10, 1), new DateTime(2026, 10, 14), "Split" },
                    { 1003, "Slavonia Test League", "2026/2027", new DateTime(2026, 11, 1), new DateTime(2026, 11, 14), "Osijek" },
                    { 1004, "Istria Test Trophy", "2026/2027", new DateTime(2026, 12, 1), new DateTime(2026, 12, 14), "Porec" }
                });

            migrationBuilder.InsertData(
                table: "GroupPhases",
                columns: new[] { "Id", "Name", "CompetitionId" },
                values: new object[,]
                {
                    { 3001, "Group A", 1001 }, { 3002, "Group B", 1001 },
                    { 3003, "Group A", 1002 }, { 3004, "Group B", 1002 },
                    { 3005, "Group A", 1003 }, { 3006, "Group B", 1003 },
                    { 3007, "Group A", 1004 }, { 3008, "Group B", 1004 }
                });
        }

        private static void InsertTeams(MigrationBuilder migrationBuilder)
        {
            var teamRows = new object[24, 6];

            for (var index = 0; index < 24; index++)
            {
                var teamNumber = index + 1;
                teamRows[index, 0] = 1100 + teamNumber;
                teamRows[index, 1] = $"Test Team {teamNumber:00}";
                teamRows[index, 2] = $"Test Coach {teamNumber:00}";
                teamRows[index, 3] = TestCities[index % TestCities.Length];
                teamRows[index, 4] = 1990 + index;
                teamRows[index, 5] = $"Test Arena {teamNumber:00}";
            }

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "Name", "CoachName", "HomeCity", "FoundedYear", "HomeArena" },
                values: teamRows);
        }

        private static void InsertPlayers(MigrationBuilder migrationBuilder)
        {
            var playerRows = new object[336, 9];
            var row = 0;

            for (var teamNumber = 1; teamNumber <= 24; teamNumber++)
            {
                for (var playerNumber = 1; playerNumber <= 14; playerNumber++)
                {
                    playerRows[row, 0] = 2000 + row + 1;
                    playerRows[row, 1] = $"Player{playerNumber:00}";
                    playerRows[row, 2] = $"Team{teamNumber:00}";
                    playerRows[row, 3] = new DateTime(1995 + playerNumber % 10, (playerNumber % 12) + 1, (playerNumber % 27) + 1);
                    playerRows[row, 4] = playerNumber;
                    playerRows[row, 5] = ((playerNumber - 1) % 7) + 1;
                    playerRows[row, 6] = 1100 + teamNumber;
                    playerRows[row, 7] = playerNumber * 2;
                    playerRows[row, 8] = playerNumber;
                    row++;
                }
            }

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "FirstName", "LastName", "BirthDate", "JerseyNumber", "Position", "TeamId", "GoalsScored", "Assists" },
                values: playerRows);
        }

        private static void InsertCompetitionTeams(MigrationBuilder migrationBuilder)
        {
            var competitionTeamRows = new object[24, 2];
            var groupTeamRows = new object[24, 2];

            for (var index = 0; index < 24; index++)
            {
                var competitionId = 1001 + index / 6;
                var teamId = 1101 + index;
                var groupId = 3001 + (index / 6 * 2) + (index % 6 < 3 ? 0 : 1);

                competitionTeamRows[index, 0] = competitionId;
                competitionTeamRows[index, 1] = teamId;
                groupTeamRows[index, 0] = groupId;
                groupTeamRows[index, 1] = teamId;
            }

            migrationBuilder.InsertData(
                table: "TeamCompetitions",
                columns: new[] { "CompetitionsId", "TeamsId" },
                values: competitionTeamRows);

            migrationBuilder.InsertData(
                table: "GroupPhaseTeams",
                columns: new[] { "GroupPhasesId", "TeamsId" },
                values: groupTeamRows);
        }

        private static void InsertCompetitionManagers(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CompetitionAdministrators",
                columns: new[] { "AdministratorsId", "ManagedCompetitionsId" },
                values: new object[,]
                {
                    { 9002, 1001 },
                    { 9003, 1002 },
                    { 9004, 1003 },
                    { 9005, 1004 }
                });
        }

        private static void InsertUserRoles(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                INSERT INTO [AspNetUserRoles] ([UserId], [RoleId])
                SELECT 9001, r.[Id]
                FROM [AspNetRoles] r
                WHERE r.[NormalizedName] = N'ADMIN'
                  AND NOT EXISTS (SELECT 1 FROM [AspNetUserRoles] WHERE [UserId] = 9001 AND [RoleId] = r.[Id]);

                INSERT INTO [AspNetUserRoles] ([UserId], [RoleId])
                SELECT u.[Id], r.[Id]
                FROM [AppUsers] u
                CROSS JOIN [AspNetRoles] r
                WHERE u.[Id] BETWEEN 9002 AND 9005
                  AND r.[NormalizedName] = N'MANAGER'
                  AND NOT EXISTS (SELECT 1 FROM [AspNetUserRoles] WHERE [UserId] = u.[Id] AND [RoleId] = r.[Id]);
                """);
        }

        private static readonly string[] TestCities =
        {
            "Zagreb",
            "Split",
            "Osijek",
            "Rijeka",
            "Porec",
            "Zadar"
        };
    }
}
