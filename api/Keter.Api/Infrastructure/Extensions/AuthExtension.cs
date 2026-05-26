// Keter.Api/Infrastructure/Extensions/AuthExtensions.cs
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Keter.Api.Infrastructure.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddKeterAuthentication(this IServiceCollection services, IConfiguration config)
    {
        // 1. Configure JWT Bearer
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            // The secret key used to sign the tokens (store this safely in appsettings/env variables!)
            var jwtSecret = config["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret is missing!");
            var key = Encoding.ASCII.GetBytes(jwtSecret);

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = config["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // Tokens expire exactly when they are supposed to
            };
        });

        // 2. Enable Authorization policies
        services.AddAuthorization();

        return services;
    }
}