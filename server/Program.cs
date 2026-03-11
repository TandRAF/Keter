var builder = WebApplication.CreateBuilder(args);

// Adăugăm controllerele (necesare pentru API-ul tău de Task-uri)
builder.Services.AddControllers();

var app = builder.Build();

// Configurăm pipeline-ul HTTP
if (app.Environment.IsDevelopment())
{
    // În .NET 8, de obicei folosim Swagger, dar pentru moment 
    // lăsăm curat ca să pornească Docker-ul fără erori
}

app.UseHttpsRedirection();

// Mapăm controllerele
app.MapControllers();

// Păstrăm exemplul de weather doar ca să verifici dacă merge
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}