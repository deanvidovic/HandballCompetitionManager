using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using HandballCompetitionManager.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HandballCompetitionManager.IntegrationTests;

public sealed class AppUsersApiTests : ApiIntegrationTestBase
{
    [Fact]
    public async Task UsersApi_ShouldSupportCrudAndValidationScenarios()
    {
        var existingId = await WithDb(db => db.AppUsers.Select(user => user.Id).FirstAsync());

        var allResponse = await Client.GetAsync("/api/users");
        Assert.Equal(HttpStatusCode.OK, allResponse.StatusCode);

        var byIdResponse = await Client.GetAsync($"/api/users/{existingId}");
        Assert.Equal(HttpStatusCode.OK, byIdResponse.StatusCode);

        var missingResponse = await Client.GetAsync("/api/users/999999");
        Assert.Equal(HttpStatusCode.NotFound, missingResponse.StatusCode);

        var invalidResponse = await Client.PostAsJsonAsync("/api/users", new
        {
            username = "bad",
            displayName = "Bad User",
            email = "not-an-email",
            oib = "123",
            jmbg = "123",
            role = UserRole.Coach,
            managedCompetitionIds = Array.Empty<int>()
        });
        Assert.Equal(HttpStatusCode.BadRequest, invalidResponse.StatusCode);

        var createResponse = await Client.PostAsJsonAsync("/api/users", new
        {
            username = "api.coach",
            displayName = "API Coach",
            email = "api.coach@test.local",
            oib = "12345678909",
            jmbg = "1234567890129",
            role = UserRole.Coach,
            dateOfBirth = new DateTime(1988, 4, 12),
            managedCompetitionIds = Array.Empty<int>()
        });
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdId = await WithDb(db => db.AppUsers.Where(user => user.Email == "api.coach@test.local").Select(user => user.Id).SingleAsync());

        var updateResponse = await Client.PutAsJsonAsync($"/api/users/{createdId}", new
        {
            username = "api.coach.updated",
            displayName = "API Coach Updated",
            email = "api.coach.updated@test.local",
            oib = "12345678908",
            jmbg = "1234567890128",
            role = UserRole.Coach,
            managedCompetitionIds = Array.Empty<int>()
        });
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        var updateMissingResponse = await Client.PutAsJsonAsync("/api/users/999999", new
        {
            username = "missing",
            displayName = "Missing User",
            email = "missing@test.local",
            oib = "12345678907",
            jmbg = "1234567890127",
            role = UserRole.Guest,
            managedCompetitionIds = Array.Empty<int>()
        });
        Assert.Equal(HttpStatusCode.NotFound, updateMissingResponse.StatusCode);

        var deleteResponse = await Client.DeleteAsync($"/api/users/{createdId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var deleteMissingResponse = await Client.DeleteAsync("/api/users/999999");
        Assert.Equal(HttpStatusCode.NotFound, deleteMissingResponse.StatusCode);
    }

    [Fact]
    public async Task UsersApi_ShouldApplyFiltersAndHideSoftDeletedUsers()
    {
        var existingId = await WithDb(db => db.AppUsers.Where(user => user.Role == UserRole.Admin).Select(user => user.Id).FirstAsync());

        var filteredResponse = await Client.GetAsync($"/api/users?role={(int)UserRole.Admin}");
        Assert.Equal(HttpStatusCode.OK, filteredResponse.StatusCode);

        using var filteredJson = JsonDocument.Parse(await filteredResponse.Content.ReadAsStringAsync());
        var users = filteredJson.RootElement.EnumerateArray().ToList();
        Assert.NotEmpty(users);
        Assert.All(users, user => Assert.Equal((int)UserRole.Admin, user.GetProperty("role").GetInt32()));

        Assert.Equal(HttpStatusCode.NoContent, (await Client.DeleteAsync($"/api/users/{existingId}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Client.GetAsync($"/api/users/{existingId}")).StatusCode);
    }
}
