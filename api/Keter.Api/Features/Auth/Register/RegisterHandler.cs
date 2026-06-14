using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Keter.Api/Features/Auth/Register/RegisterHandler.cs
using MediatR;
using Keter.Domain.Entities;
using Keter.Api.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;

namespace Keter.Api.Features.Auth.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, Guid>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public RegisterHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // 1. Create the Identity User
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        // This hashes the password and saves to the DB automatically
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            // In a production app, you'd format these errors nicely.
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Registration failed: {errors}");
        }

        // 2. Create the linked Profile
        var profile = new Profile
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            FullName = request.FullName
        };

        _context.Profiles.Add(profile);
        await _context.SaveChangesAsync(cancellationToken);

        return profile.Id;
    }
}