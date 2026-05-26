// Keter.Api/Program.cs
using Keter.Api.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// --- 1. DEPENDENCY INJECTION ---
builder.Services
    .AddKeterAuthentication(builder.Configuration) // Injects JWT & Identity
    .AddVerticalSliceCore();                       // Injects Newtonsoft & MediatR

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 2. MIDDLEWARE PIPELINE ---
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// SECURITY: Authentication must always come BEFORE Authorization!
app.UseAuthentication(); 
app.UseAuthorization();  

// Map the endpoints
app.MapControllers(); 

app.Run();