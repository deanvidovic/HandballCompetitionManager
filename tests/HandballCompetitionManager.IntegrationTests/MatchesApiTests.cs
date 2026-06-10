using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using HandballCompetitionManager.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HandballCompetitionManager.IntegrationTests;

public sealed class MatchesApiTests : ApiIntegrationTestBase
{
    [Fact]
    public async Task MatchesApi_ShouldSupportCrudAndValidationScenarios()
    {
        var seed = await WithDb(async db => new
        {
            MatchId = await db.Matches.Select(match => match.Id).FirstAsync(),
            CompetitionId = await db.Competitions.Select(competition => competition.Id).FirstAsync(),
            GroupId = await db.GroupPhases.Select(group => group.Id).FirstAsync(),
            TeamIds = await db.Teams.Select(team => team.Id).Take(2).ToListAsync()
        });

        Assert.Equal(HttpStatusCode.OK, (await Client.GetAsync("/api/matches")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await Client.GetAsync($"/api/matches/{seed.MatchId}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Client.GetAsync("/api/matches/999999")).StatusCode);

        var invalidResponse = await Client.PostAsJsonAsync("/api/matches", new
        {
            competitionId = seed.CompetitionId,
            groupId = seed.GroupId,
            roundNumber = 1,
            kickoff = new DateTime(2026, 6, 3, 18, 0, 0),
            homeTeamId = seed.TeamIds[0],
            awayTeamId = seed.TeamIds[0],
            homeScore = 0,
            awayScore = 0,
            maintenanceHall = "Arena",
            status = MatchStatus.Scheduled
        });
        Assert.Equal(HttpStatusCode.BadRequest, invalidResponse.StatusCode);

        var createResponse = await Client.PostAsJsonAsync("/api/matches", new
        {
            competitionId = seed.CompetitionId,
            groupId = seed.GroupId,
            roundNumber = 2,
            kickoff = new DateTime(2026, 6, 4, 18, 0, 0),
            homeTeamId = seed.TeamIds[0],
            awayTeamId = seed.TeamIds[1],
            homeScore = 0,
            awayScore = 0,
            maintenanceHall = "API Arena",
            status = MatchStatus.Scheduled
        });
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdId = await WithDb(db => db.Matches.Where(match => match.RoundNumber == 2).Select(match => match.Id).SingleAsync());

        var updateResponse = await Client.PutAsJsonAsync($"/api/matches/{createdId}", new
        {
            competitionId = seed.CompetitionId,
            groupId = seed.GroupId,
            roundNumber = 2,
            kickoff = new DateTime(2026, 6, 4, 18, 0, 0),
            homeTeamId = seed.TeamIds[0],
            awayTeamId = seed.TeamIds[1],
            homeScore = 30,
            awayScore = 29,
            maintenanceHall = "API Arena",
            status = MatchStatus.Finished
        });
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        Assert.Equal(HttpStatusCode.NotFound, (await Client.PutAsJsonAsync("/api/matches/999999", new
        {
            competitionId = seed.CompetitionId,
            groupId = seed.GroupId,
            roundNumber = 2,
            kickoff = new DateTime(2026, 6, 4, 18, 0, 0),
            homeTeamId = seed.TeamIds[0],
            awayTeamId = seed.TeamIds[1],
            homeScore = 30,
            awayScore = 29,
            maintenanceHall = "API Arena",
            status = MatchStatus.Finished
        })).StatusCode);

        Assert.Equal(HttpStatusCode.NoContent, (await Client.DeleteAsync($"/api/matches/{createdId}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Client.DeleteAsync("/api/matches/999999")).StatusCode);
    }

    [Fact]
    public async Task MatchesApi_ShouldApplyFiltersAndDeleteMatches()
    {
        var seed = await WithDb(async db => new
        {
            MatchId = await db.Matches.Select(match => match.Id).FirstAsync(),
            CompetitionId = await db.Competitions.Select(competition => competition.Id).FirstAsync(),
            TeamId = await db.Teams.Select(team => team.Id).FirstAsync()
        });

        var filteredResponse = await Client.GetAsync($"/api/matches?competitionId={seed.CompetitionId}&teamId={seed.TeamId}&status={(int)MatchStatus.Finished}");
        Assert.Equal(HttpStatusCode.OK, filteredResponse.StatusCode);

        using var filteredJson = JsonDocument.Parse(await filteredResponse.Content.ReadAsStringAsync());
        var matches = filteredJson.RootElement.EnumerateArray().ToList();
        Assert.NotEmpty(matches);
        Assert.All(matches, match =>
        {
            Assert.Equal(seed.CompetitionId, match.GetProperty("competition").GetProperty("id").GetInt32());
            Assert.Equal((int)MatchStatus.Finished, match.GetProperty("status").GetInt32());
            var homeTeamId = match.GetProperty("homeTeam").GetProperty("id").GetInt32();
            var awayTeamId = match.GetProperty("awayTeam").GetProperty("id").GetInt32();
            Assert.True(homeTeamId == seed.TeamId || awayTeamId == seed.TeamId);
        });

        Assert.Equal(HttpStatusCode.NoContent, (await Client.DeleteAsync($"/api/matches/{seed.MatchId}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Client.GetAsync($"/api/matches/{seed.MatchId}")).StatusCode);
    }
}
