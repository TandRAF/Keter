using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Keter.Api/Infrastructure/Auth/IJwtProvider.cs
using Keter.Domain.Entities;

// Keter.Api/Infrastructure/Auth/JwtProvider.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Keter.Api.Infrastructure.Auth;

public sealed class JwtProvider : IJwtProvider
{
    private readonly IConfiguration _configuration;

    public JwtProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Generate(ApplicationUser user)
    {
        // 1. Get the secret key from appsettings.json or Docker env variables
        var secretKey = _configuration["Jwt:Secret"] 
            ?? throw new InvalidOperationException("JWT Secret is missing");
            
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // 2. Embed the User's Data inside the Token (Claims)
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id), // Standard ID claim
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim("keter_user_id", user.Id) // Custom claim if needed
        };

        // 3. Construct the Token
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2), // Token lives for 2 hours
            signingCredentials: credentials);

        // 4. Return the string token
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}