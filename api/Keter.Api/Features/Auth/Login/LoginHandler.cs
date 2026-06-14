using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// Keter.Api/Features/Auth/Login/LoginHandler.cs
using MediatR;
using Keter.Domain.Entities;
using Keter.Api.Infrastructure.Auth;
using Microsoft.AspNetCore.Identity;

namespace Keter.Api.Features.Auth.Login;

public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtProvider _jwtProvider;

    public LoginHandler(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Find the user
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        // 2. Verify password (returning generic errors prevents email enumeration tracking)
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        // 3. Generate the Token
        var token = _jwtProvider.Generate(user);

        return new LoginResponse(token, user.Email!);
    }
}