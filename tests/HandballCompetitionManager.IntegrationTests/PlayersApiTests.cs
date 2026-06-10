using System.Net;
using System.Net.Http.Json;
using HandballCompetitionManager.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HandballCompetitionManager.IntegrationTests;

public sealed class PlayersApiTests : ApiIntegrationTestBase
{
    [Fact]
    public async Task PlayersApi_ShouldSupportCrudAndValidationScenarios()
    {
        var seed = await WithDb(async db => new
        {
            PlayerId = await db.Players.Select(player => player.Id).FirstAsync(),
            TeamId = await db.Teams.Select(team => team.Id).FirstAsync()
        });

        Assert.Equal(HttpStatusCode.OK, (await Client.GetAsync("/api/players")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await Client.GetAsync($"/api/players/{seed.PlayerId}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Client.GetAsync("/api/players/999999")).StatusCode);

        var invalidResponse = await Client.PostAsJsonAsync("/api/players", new
        {
            firstName = "",
            lastName = "Invalid",
            birthDate = new DateTime(2000, 1, 1),
            jerseyNumber = 200,
            position = PlayerPosition.Pivot,
            teamId = 999999,
            goalsScored = -1,
            assists = 0
        });
        Assert.Equal(HttpStatusCode.BadRequest, invalidResponse.StatusCode);

        var createResponse = await Client.PostAsJsonAsync("/api/players", new
        {
            firstName = "API",
            lastName = "Player",
            birthDate = new DateTime(2001, 2, 3),
            jerseyNumber = 11,
            position = PlayerPosition.RightBack,
            teamId = seed.TeamId,
            goalsScored = 4,
            assists = 2
        });
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdId = await WithDb(db => db.Players.Where(player => player.FirstName == "API").Select(player => player.Id).SingleAsync());

        var updateResponse = await Client.PutAsJsonAsync($"/api/players/{createdId}", new
        {
            firstName = "API",
            lastName = "Player Updated",
            birthDate = new DateTime(2001, 2, 3),
            jerseyNumber = 12,
            position = PlayerPosition.LeftBack,
            teamId = seed.TeamId,
            goalsScored = 8,
            assists = 3
        });
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        Assert.Equal(HttpStatusCode.NotFound, (await Client.PutAsJsonAsync("/api/players/999999", new
        {
            firstName = "Missing",
            lastName = "Player",
            birthDate = new DateTime(2001, 2, 3),
            jerseyNumber = 12,
            position = PlayerPosition.LeftBack,
            teamId = seed.TeamId,
            goalsScored = 8,
            assists = 3
        })).StatusCode);

        Assert.Equal(HttpStatusCode.NoContent, (await Client.DeleteAsync($"/api/players/{createdId}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Client.DeleteAsync("/api/players/999999")).StatusCode);
    }

    [Fact]
    public async Task PlayersApi_ShouldApplyFiltersAndHideSoftDeletedPlayers()
    {
        var seed = await WithDb(async db => new
        {
            PlayerId = await db.Players.Select(player => player.Id).FirstAsync(),
            TeamId = await db.Teams.Select(team => team.Id).FirstAsync()
        });

        var filteredResponse = await Client.GetAsync($"/api/players?teamId={seed.TeamId}");
        Assert.Equal(HttpStatusCode.OK, filteredResponse.StatusCode);

        Assert.Equal(HttpStatusCode.NoContent, (await Client.DeleteAsync($"/api/players/{seed.PlayerId}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Client.GetAsync($"/api/players/{seed.PlayerId}")).StatusCode);
    }
}
