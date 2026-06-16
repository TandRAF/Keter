// Keter.Api.IntegrationTests/Features/Workspaces/DeleteWorkspaceEndpointTests.cs
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using Keter.Api.Infrastructure.Database;
using Keter.Domain.Entities;

namespace Keter.Api.IntegrationTests.Features.Workspaces;

public class DeleteWorkspaceEndpointTests : IClassFixture<KeterApiFactory>
{
    private readonly KeterApiFactory _factory;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;

    public DeleteWorkspaceEndpointTests(KeterApiFactory factory, ITestOutputHelper output)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _output = output;
    }

    [Fact]
    public async Task DeleteWorkspace_AsAdmin_ReturnsNoContent_AndDeletesFromDatabase()
    {
        _output.WriteLine("▶ 1. Arrange: Seeding database with a fake User, Workspace, and Admin role...");
        var workspaceId = Guid.NewGuid();

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            // 🌟 STEP 1: Save the User FIRST
            if (!db.Users.Any(u => u.Id == TestAuthHandler.TestUserId))
            {
                db.Users.Add(new Keter.Domain.Entities.ApplicationUser { Id = TestAuthHandler.TestUserId, UserName = "test@keter.com", Email = "test@keter.com" });
                await db.SaveChangesAsync(); 
            }

            // 🌟 STEP 2: Now it is safe to save the Workspace and Member
            db.Workspaces.Add(new Workspace { Id = workspaceId, Name = "Test Deletion Workspace", CreatedAt = DateTime.UtcNow });
            db.WorkspaceMembers.Add(new WorkspaceMember { WorkspaceId = workspaceId, UserId = TestAuthHandler.TestUserId, Role = "Admin" });
            await db.SaveChangesAsync();
        }

        // 🌟 USE THE "Test" SCHEME
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Test", "token");

        // ... Keep the rest of your test exactly the same (DeleteAsync, etc.)

        var response = await _client.DeleteAsync($"/api/workspaces/{workspaceId}");

        _output.WriteLine($"===== WHAT HAPPENED =====");
        _output.WriteLine($"Status Code: {(int)response.StatusCode} {response.StatusCode}");
        _output.WriteLine($"=========================\n");

        _output.WriteLine("▶ 3. Assert: Verifying the API returned 204 No Content...");
        // 204 No Content is the standard success response for a DELETE operation
        response.EnsureSuccessStatusCode(); 
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);

        _output.WriteLine("▶ 4. Assert: Querying the database to prove it was actually deleted...");
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var workspaceExists = await db.Workspaces.AnyAsync(w => w.Id == workspaceId);
            
            Assert.False(workspaceExists, "The workspace should have been deleted from the database!");
        }

        _output.WriteLine("✔ Test completed successfully! The workspace was permanently removed.");
    }
}