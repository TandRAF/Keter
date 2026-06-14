// Keter.Api.IntegrationTests/Features/Auth/RegisterEndpointTests.cs
using System.Net.Http.Json;
using Xunit;
using Keter.Api.Features.Auth.Register; 

namespace Keter.Api.IntegrationTests.Features.Auth;

public class RegisterEndpointTests : IClassFixture<KeterApiFactory>
{
    private readonly HttpClient _client;

    public RegisterEndpointTests(KeterApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsOk()
    {
        // Arrange
        var request = new RegisterRequest("testuser@keter.com", "StrongPassword123!", "Test User");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<RegisterResponseDto>();
        
        Assert.NotNull(result);
        Assert.Equal("Registration successful.", result.Message);
    }
    
    private record RegisterResponseDto(Guid ProfileId, string Message);
}