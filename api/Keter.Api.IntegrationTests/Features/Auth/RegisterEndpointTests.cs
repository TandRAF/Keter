// Keter.Api.IntegrationTests/Features/Auth/RegisterEndpointTests.cs
using System.Net.Http.Json;
using Xunit;
using Xunit.Abstractions; // Adăugat pentru logging vizual în terminal
using Keter.Api.Features.Auth.Register; 

namespace Keter.Api.IntegrationTests.Features.Auth;

public class RegisterEndpointTests : IClassFixture<KeterApiFactory>
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;

    // Injectăm KeterApiFactory (pentru baza de date Docker) și ITestOutputHelper (pentru loguri)
    public RegisterEndpointTests(KeterApiFactory factory, ITestOutputHelper output)
    {
        _client = factory.CreateClient();
        _output = output;
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsOk()
    {
        _output.WriteLine("▶ 1. Inițializare date pentru înregistrare (Arrange)...");
        // Arrange
        var request = new RegisterRequest("testuser@keter.com", "StrongPassword123!", "Test User");

        _output.WriteLine($"▶ 2. Trimitere payload valid pentru {request.Email} către /api/auth/register (Act)...");
        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        _output.WriteLine("▶ 3. Verificare dacă serverul a răspuns cu 200 OK (Assert)...");
        // Assert
        response.EnsureSuccessStatusCode();
        
        _output.WriteLine("▶ 4. Deserializare răspuns și validare structură JSON...");
        var result = await response.Content.ReadFromJsonAsync<RegisterResponseDto>();
        
        Assert.NotNull(result);
        Assert.Equal("Registration successful.", result.Message);
        
        _output.WriteLine("✔ Test finalizat cu succes! Profilul a fost creat în baza de date temporară.");
    }
    
    private record RegisterResponseDto(Guid ProfileId, string Message);
}