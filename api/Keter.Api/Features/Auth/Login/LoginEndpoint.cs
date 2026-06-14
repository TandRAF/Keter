using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Keter.Api/Features/Auth/Login/LoginEndpoint.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Keter.Api.Features.Auth.Login;

public static class LoginEndpoint
{
    public static void MapLoginEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async (
            [FromBody] LoginRequest request, 
            [FromServices] IMediator mediator) =>
        {
            var command = new LoginCommand(request.Email, request.Password);
            var response = await mediator.Send(command);
            
            return Results.Ok(response);
        })
        .WithTags("Auth")
        .AllowAnonymous(); 
    }
}