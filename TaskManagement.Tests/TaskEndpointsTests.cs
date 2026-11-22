using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TaskManagement.Application.DTOs;

namespace TaskManagement.Tests;

public class TaskEndpointsTests
{
    [Fact]
    public async Task ListTasks_WithoutToken_ReturnUnauthorized()
    {
        using var factory = new TaskApiFactory();
        using HttpClient client = factory.CreateClient();

        using var response = await client.GetAsync("/api/tasks");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdateTask_WhenOwnedByAnotherUser_ReturnsNotFoundAndKeepsTitle()
    {
        // Arrange
        using var factory = new TaskApiFactory();
        using var ownerClient = factory.CreateClient();
        using var otherClient = factory.CreateClient();

        var ownerToken = await RegisterAndLoginAsync(ownerClient);
        var otherToken = await RegisterAndLoginAsync(otherClient);

        ownerClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", ownerToken);

        otherClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", otherToken);

        const string originalTitle = "Original title";

        using var createResponse = await ownerClient.PostAsJsonAsync(
            "/api/tasks",
            new { title = originalTitle });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponse>();
        Assert.NotNull(createdTask);

        // Act
        using var updateResponse = await otherClient.PutAsJsonAsync(
            $"/api/tasks/{createdTask.Id}",
            new { title = "Changed by another user", priority = 1, isCompleted = false });

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, updateResponse.StatusCode);

        var ownerTasks = await ownerClient.GetFromJsonAsync<List<TaskResponse>>("/api/tasks");
        Assert.NotNull(ownerTasks);

        var savedTask = Assert.Single(ownerTasks, task => task.Id == createdTask.Id);
        Assert.Equal(originalTitle, savedTask.Title);
    }

    private static async Task<string> RegisterAndLoginAsync(HttpClient client)
    {
        var username = $"tester_{Guid.NewGuid():N}";
        var email = $"{username}@example.test";
        var password = Guid.NewGuid().ToString("N");

        using var registerResponse = await client.PostAsJsonAsync(
            "/api/auth/register",
            new { username, email, password });

        Assert.Equal(HttpStatusCode.Created, registerResponse.StatusCode);

        using var loginResponse = await client.PostAsJsonAsync(
            "/api/auth/login",
            new { email, password });

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        var login = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(login);
        Assert.False(string.IsNullOrWhiteSpace(login.AccessToken));

        return login.AccessToken;
    }
}
