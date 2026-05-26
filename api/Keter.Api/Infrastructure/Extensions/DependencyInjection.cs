// Keter.Api/Infrastructure/Extensions/DependencyInjection.cs
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Keter.Api.Infrastructure.Behaviors; // Add this using statement!

namespace Keter.Api.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddKeterInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // 1. Register MediatR AND the Validation Behavior
        services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            
            // This is the magic line! It connects MediatR to FluentValidation.
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>)); 
        });

        // 2. Register FluentValidation (Scans for your Validators)
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        return services;
    }
}