// Keter.Api/Infrastructure/Extensions/DatabaseInitializationExtension.cs
using Keter.Api.Infrastructure.Database;
using Keter.Api.Infrastructure.Database.Seeding;
using Microsoft.EntityFrameworkCore;
using Npgsql; // Dacă folosești PostgreSQL

namespace Keter.Api.Infrastructure.Extensions;

public static class DatabaseInitializationExtension
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();

        // Încercăm de 5 ori, cu o pauză de 3 secunde între încercări
        int maxRetries = 5;
        int delayInSeconds = 3;

        for (int i = 1; i <= maxRetries; i++)
        {
            try
            {
                logger.LogInformation("Se încearcă migrarea bazei de date (Încercarea {Current} din {Max})...", i, maxRetries);
                
                var context = services.GetRequiredService<ApplicationDbContext>();

                if (context.Database.IsRelational())
                {
                    // Această linie înlocuiește complet 'dotnet ef database update'
                    await context.Database.MigrateAsync();
                }

                // Dacă migrarea a reușit, rulăm seed-ul
                var seeder = services.GetRequiredService<DatabaseSeeder>();
                await seeder.SeedAsync();

                logger.LogInformation("Baza de date a fost inițializată și populată cu succes!");
                break; // Ieșim din loop pentru că totul a funcționat
            }
            catch (Exception ex)
            {
                logger.LogWarning("Baza de date nu este pregătită încă. Eroare: {Message}", ex.Message);

                if (i == maxRetries)
                {
                    logger.LogError(ex, "Eroare fatală. S-a atins numărul maxim de reîncercări pentru inițializarea bazei de date.");
                    throw; // Crăpăm aplicația dacă după 5 încercări tot nu merge
                }

                // Așteptăm înainte de următoarea încercare
                await Task.Delay(TimeSpan.FromSeconds(delayInSeconds));
            }
        }
    }
}