// Keter.Api.IntegrationTests/Features/Workspaces/CreateWorkspaceEndpointTests.cs
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using Keter.Api.Infrastructure.Database;
using Keter.Api.Features.Workspaces.CreateWorkspace;

namespace Keter.Api.IntegrationTests.Features.Workspaces;

public class CreateWorkspaceEndpointTests : IClassFixture<KeterApiFactory>
{
    private readonly KeterApiFactory _factory;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;

    public CreateWorkspaceEndpointTests(KeterApiFactory factory, ITestOutputHelper output)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _output = output;
    }

   [Fact]
    public async Task CreateWorkspace_WithValidData_ReturnsCreated_AndSavesToDatabase()
    {
        _output.WriteLine("▶ 1. Arrange: Setting up test data and authentication...");
        
        // 🌟 SEED THE USER HERE (After migrations are finished)
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (!db.Users.Any(u => u.Id == TestAuthHandler.TestUserId))
            {
                db.Users.Add(new Keter.Domain.Entities.ApplicationUser { Id = TestAuthHandler.TestUserId, UserName = "test@keter.com", Email = "test@keter.com" });
                await db.SaveChangesAsync();
            }
        }

        var requestPayload = new CreateWorkspaceRequest("Rafael's New Workspace");

        // 🌟 USE THE "Test" SCHEME WITH A DUMMY TOKEN STRING
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Test", "token");

        // ... Keep the rest of your test exactly the same (PostAsJsonAsync, etc.)

        _output.WriteLine("===== WHAT I AM SENDING =====");
        _output.WriteLine("Endpoint: POST /api/workspaces");
        _output.WriteLine(JsonSerializer.Serialize(requestPayload, new JsonSerializerOptions { WriteIndented = true }));
        _output.WriteLine("=============================\n");

        _output.WriteLine("▶ 2. Act: Sending request to the Docker database...");
        var response = await _client.PostAsJsonAsync("/api/workspaces", requestPayload);

        var responseBody = await response.Content.ReadAsStringAsync();

        _output.WriteLine("===== WHAT HAPPENED =====");
        _output.WriteLine($"Status Code: {(int)response.StatusCode} {response.StatusCode}");
        _output.WriteLine("Response Body:");
        _output.WriteLine(responseBody);
        _output.WriteLine("=========================\n");

        _output.WriteLine("▶ 3. Assert: Verifying the API returned 201 Created...");
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

        // Extract the ID of the newly created workspace from the JSON response
        var result = JsonSerializer.Deserialize<CreateWorkspaceResponseDto>(
            responseBody, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);

        _output.WriteLine("▶ 4. Assert: Querying PostgreSQL to verify the relationships were created...");
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            // Verify 1: Did it create the workspace?
            var workspaceInDb = await db.Workspaces.FirstOrDefaultAsync(w => w.Id == result.Id);
            Assert.NotNull(workspaceInDb);
            Assert.Equal("Rafael's New Workspace", workspaceInDb.Name);

            // Verify 2: Did it assign the creator as the Admin?
            // ✅ CORRECT (Uses our global test user ID)
            var memberLink = await db.WorkspaceMembers.FirstOrDefaultAsync(m => m.WorkspaceId == result.Id && m.UserId == TestAuthHandler.TestUserId);
            Assert.NotNull(memberLink);
            Assert.Equal("Admin", memberLink.Role);
        }

        _output.WriteLine("✔ Test completed successfully! The workspace and admin role are fully integrated.");
    }

    // A tiny record just to deserialize the JSON response
    private record CreateWorkspaceResponseDto(Guid Id);
}