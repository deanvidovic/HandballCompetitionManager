using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HandballCompetitionManager.IntegrationTests;

public sealed class TeamsApiTests : ApiIntegrationTestBase
{
    [Fact]
    public async Task TeamsApi_ShouldSupportCrudAndValidationScenarios()
    {
        var existingId = await WithDb(db => db.Teams.Select(team => team.Id).FirstAsync());

        Assert.Equal(HttpStatusCode.OK, (await Client.GetAsync("/api/teams")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await Client.GetAsync($"/api/teams/{existingId}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Client.GetAsync("/api/teams/999999")).StatusCode);

        var invalidResponse = await Client.PostAsJsonAsync("/api/teams", new
        {
            name = "",
            coachName = "Coach",
            homeCity = "Zagreb",
            foundedYear = 1700,
            homeArena = "Arena",
            competitionIds = Array.Empty<int>(),
            groupPhaseIds = Array.Empty<int>()
        });
        Assert.Equal(HttpStatusCode.BadRequest, invalidResponse.StatusCode);

        var createResponse = await Client.PostAsJsonAsync("/api/teams", new
        {
            name = "RK API Team",
            coachName = "Coach API",
            homeCity = "Rijeka",
            foundedYear = 2015,
            homeArena = "API Arena",
            competitionIds = Array.Empty<int>(),
            groupPhaseIds = Array.Empty<int>()
        });
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdId = await WithDb(db => db.Teams.Where(team => team.Name == "RK API Team").Select(team => team.Id).SingleAsync());

        var updateResponse = await Client.PutAsJsonAsync($"/api/teams/{createdId}", new
        {
            name = "RK API Team Updated",
            coachName = "Coach API",
            homeCity = "Rijeka",
            foundedYear = 2016,
            homeArena = "Updated Arena",
            competitionIds = Array.Empty<int>(),
            groupPhaseIds = Array.Empty<int>()
        });
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        Assert.Equal(HttpStatusCode.NotFound, (await Client.PutAsJsonAsync("/api/teams/999999", new
        {
            name = "Missing Team",
            coachName = "Coach Missing",
            homeCity = "Zagreb",
            foundedYear = 2010,
            homeArena = "Missing Arena",
            competitionIds = Array.Empty<int>(),
            groupPhaseIds = Array.Empty<int>()
        })).StatusCode);

        Assert.Equal(HttpStatusCode.NoContent, (await Client.DeleteAsync($"/api/teams/{createdId}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Client.DeleteAsync("/api/teams/999999")).StatusCode);
    }

    [Fact]
    public async Task TeamsApi_ShouldApplyFiltersAndHideSoftDeletedTeams()
    {
        var existingId = await WithDb(db => db.Teams.Where(team => team.HomeCity == "Zagreb").Select(team => team.Id).FirstAsync());

        var filteredResponse = await Client.GetAsync("/api/teams?city=Zagreb&foundedAfter=2000");
        Assert.Equal(HttpStatusCode.OK, filteredResponse.StatusCode);

        using var filteredJson = JsonDocument.Parse(await filteredResponse.Content.ReadAsStringAsync());
        var teams = filteredJson.RootElement.EnumerateArray().ToList();
        Assert.NotEmpty(teams);
        Assert.All(teams, team =>
        {
            Assert.Equal("Zagreb", team.GetProperty("homeCity").GetString());
            Assert.True(team.GetProperty("foundedYear").GetInt32() >= 2000);
        });

        Assert.Equal(HttpStatusCode.NoContent, (await Client.DeleteAsync($"/api/teams/{existingId}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Client.GetAsync($"/api/teams/{existingId}")).StatusCode);
    }
}
