// Keter.Api/Program.cs
using Microsoft.EntityFrameworkCore;
using Keter.Api.Infrastructure.Database;
using Keter.Api.Infrastructure.Extensions;
using Keter.Api.Features.Auth.Register;
using Keter.Api.Infrastructure.Database.Seeding;
using Keter.Api.Features.Auth.Login;
using Keter.Api.Features.Workspaces.CreateWorkspace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                           ?? "Host=localhost;Database=dummy;Username=postgres;Password=postgres"; 

    options.UseNpgsql(connectionString);
});

// --- 1. DEPENDENCY INJECTION ---
builder.Services
    .AddKeterExceptionHandling() // 1. Adaugă extensia creată la Pasul 3
    .AddKeterInfrastructure(builder.Configuration)
    .AddKeterAuthentication(builder.Configuration)
    .AddVerticalSliceCore()  
    .AddScoped<DatabaseSeeder>();       // Injects Newtonsoft & MediatR

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 2. MIDDLEWARE PIPELINE ---
app.UseSwagger();
app.UseSwaggerUI();
app.UseExceptionHandler(); 
app.UseHttpsRedirection();
app.UseHttpsRedirection();
// SECURITY: Authentication must always come BEFORE Authorization!
app.UseAuthentication(); 
app.UseAuthorization();  

// Map the endpoints
app.MapControllers(); 

//Auth Endpoints
app.MapRegisterEndpoint();
app.MapLoginEndpoint();

// Workspace Endpoints
app.MapCreateWorkspaceEndpoint();
await app.InitializeDatabaseAsync();
await app.SeedDatabaseAsync();

app.Run();

public partial class Program { }