using Keter.Api.Infrastructure.Database.Seeding;

namespace Keter.Api.Infrastructure.Extensions;

public static class DatabaseSeederExtension
{
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        // Creăm un scope pentru a extrage serviciile necesare din Dependency Injection
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var seeder = services.GetRequiredService<DatabaseSeeder>();
            await seeder.SeedAsync();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "A apărut o eroare în timpul seed-ului bazei de date.");
        }
    }
}