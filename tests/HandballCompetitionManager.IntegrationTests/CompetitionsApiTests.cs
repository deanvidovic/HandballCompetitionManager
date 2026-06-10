using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HandballCompetitionManager.IntegrationTests;

public sealed class CompetitionsApiTests : ApiIntegrationTestBase
{
    [Fact]
    public async Task CompetitionsApi_ShouldSupportCrudAndValidationScenarios()
    {
        var seed = await WithDb(async db => new
        {
            CompetitionId = await db.Competitions.Select(competition => competition.Id).FirstAsync(),
            TeamIds = await db.Teams.Select(team => team.Id).Take(2).ToListAsync(),
            AdminId = await db.AppUsers.Select(user => user.Id).FirstAsync()
        });

        Assert.Equal(HttpStatusCode.OK, (await Client.GetAsync("/api/competitions")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await Client.GetAsync($"/api/competitions/{seed.CompetitionId}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Client.GetAsync("/api/competitions/999999")).StatusCode);

        var invalidResponse = await Client.PostAsJsonAsync("/api/competitions", new
        {
            name = "Bad Competition",
            season = "2025/2026",
            startDate = new DateTime(2026, 7, 10),
            endDate = new DateTime(2026, 7, 1),
            city = "Zagreb",
            teamIds = seed.TeamIds,
            administratorIds = new[] { seed.AdminId }
        });
        Assert.Equal(HttpStatusCode.BadRequest, invalidResponse.StatusCode);

        var createResponse = await Client.PostAsJsonAsync("/api/competitions", new
        {
            name = "API Competition",
            season = "2025/2026",
            startDate = new DateTime(2026, 9, 1),
            endDate = new DateTime(2026, 9, 10),
            city = "Zadar",
            teamIds = seed.TeamIds,
            administratorIds = new[] { seed.AdminId }
        });
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdId = await WithDb(db => db.Competitions.Where(competition => competition.Name == "API Competition").Select(competition => competition.Id).SingleAsync());

        var updateResponse = await Client.PutAsJsonAsync($"/api/competitions/{createdId}", new
        {
            name = "API Competition Updated",
            season = "2025/2026",
            startDate = new DateTime(2026, 9, 2),
            endDate = new DateTime(2026, 9, 11),
            city = "Zadar",
            teamIds = seed.TeamIds,
            administratorIds = new[] { seed.AdminId }
        });
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        Assert.Equal(HttpStatusCode.NotFound, (await Client.PutAsJsonAsync("/api/competitions/999999", new
        {
            name = "Missing Competition",
            season = "2025/2026",
            startDate = new DateTime(2026, 9, 2),
            endDate = new DateTime(2026, 9, 11),
            city = "Zadar",
            teamIds = seed.TeamIds,
            administratorIds = new[] { seed.AdminId }
        })).StatusCode);

        Assert.Equal(HttpStatusCode.NoContent, (await Client.DeleteAsync($"/api/competitions/{createdId}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Client.DeleteAsync("/api/competitions/999999")).StatusCode);
    }

    [Fact]
    public async Task CompetitionsApi_ShouldApplyFiltersAndHideSoftDeletedCompetitions()
    {
        var existingId = await WithDb(db => db.Competitions.Where(competition => competition.City == "Zagreb").Select(competition => competition.Id).FirstAsync());

        var filteredResponse = await Client.GetAsync("/api/competitions?city=Zagreb&season=2025%2F2026");
        Assert.Equal(HttpStatusCode.OK, filteredResponse.StatusCode);

        using var filteredJson = JsonDocument.Parse(await filteredResponse.Content.ReadAsStringAsync());
        var competitions = filteredJson.RootElement.EnumerateArray().ToList();
        Assert.NotEmpty(competitions);
        Assert.All(competitions, competition =>
        {
            Assert.Equal("Zagreb", competition.GetProperty("city").GetString());
            Assert.Equal("2025/2026", competition.GetProperty("season").GetString());
        });

        Assert.Equal(HttpStatusCode.NoContent, (await Client.DeleteAsync($"/api/competitions/{existingId}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Client.GetAsync($"/api/competitions/{existingId}")).StatusCode);
    }
}
