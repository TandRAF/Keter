using System.Text;
using Keter.Api.Infrastructure.Database; // Needed for the DbContext
using Keter.Domain.Entities;             // Needed for ApplicationUser
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;     // Needed for Identity setup
using Microsoft.IdentityModel.Tokens;
using Keter.Api.Infrastructure.Auth;

namespace Keter.Api.Infrastructure.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddKeterAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>() 
        .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
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
                ClockSkew = TimeSpan.Zero
            };
        });
        services.AddAuthorization();

        return services;
    }
}