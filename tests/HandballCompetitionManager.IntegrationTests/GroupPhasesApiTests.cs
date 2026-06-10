using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HandballCompetitionManager.IntegrationTests;

public sealed class GroupPhasesApiTests : ApiIntegrationTestBase
{
    [Fact]
    public async Task GroupsApi_ShouldSupportCrudAndValidationScenarios()
    {
        var seed = await WithDb(async db => new
        {
            GroupId = await db.GroupPhases.Select(group => group.Id).FirstAsync(),
            CompetitionId = await db.Competitions.Select(competition => competition.Id).FirstAsync(),
            TeamIds = await db.Teams.Select(team => team.Id).Take(2).ToListAsync()
        });

        Assert.Equal(HttpStatusCode.OK, (await Client.GetAsync("/api/groups")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await Client.GetAsync($"/api/groups/{seed.GroupId}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Client.GetAsync("/api/groups/999999")).StatusCode);

        var invalidResponse = await Client.PostAsJsonAsync("/api/groups", new
        {
            name = "Bad Group",
            competitionId = 999999,
            teamIds = seed.TeamIds
        });
        Assert.Equal(HttpStatusCode.BadRequest, invalidResponse.StatusCode);

        var createResponse = await Client.PostAsJsonAsync("/api/groups", new
        {
            name = "API Group",
            competitionId = seed.CompetitionId,
            teamIds = seed.TeamIds
        });
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdId = await WithDb(db => db.GroupPhases.Where(group => group.Name == "API Group").Select(group => group.Id).SingleAsync());

        var updateResponse = await Client.PutAsJsonAsync($"/api/groups/{createdId}", new
        {
            name = "API Group Updated",
            competitionId = seed.CompetitionId,
            teamIds = seed.TeamIds
        });
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        Assert.Equal(HttpStatusCode.NotFound, (await Client.PutAsJsonAsync("/api/groups/999999", new
        {
            name = "Missing Group",
            competitionId = seed.CompetitionId,
            teamIds = seed.TeamIds
        })).StatusCode);

        Assert.Equal(HttpStatusCode.NoContent, (await Client.DeleteAsync($"/api/groups/{createdId}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Client.DeleteAsync("/api/groups/999999")).StatusCode);
    }

    [Fact]
    public async Task GroupsApi_ShouldApplyFiltersAndDeleteGroups()
    {
        var seed = await WithDb(async db => new
        {
            GroupId = await db.GroupPhases.Select(group => group.Id).FirstAsync(),
            CompetitionId = await db.Competitions.Select(competition => competition.Id).FirstAsync()
        });

        var filteredResponse = await Client.GetAsync($"/api/groups?competitionId={seed.CompetitionId}");
        Assert.Equal(HttpStatusCode.OK, filteredResponse.StatusCode);

        using var filteredJson = JsonDocument.Parse(await filteredResponse.Content.ReadAsStringAsync());
        var groups = filteredJson.RootElement.EnumerateArray().ToList();
        Assert.NotEmpty(groups);
        Assert.All(groups, group => Assert.Equal(seed.CompetitionId, group.GetProperty("competition").GetProperty("id").GetInt32()));

        Assert.Equal(HttpStatusCode.NoContent, (await Client.DeleteAsync($"/api/groups/{seed.GroupId}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Client.GetAsync($"/api/groups/{seed.GroupId}")).StatusCode);
    }
}
