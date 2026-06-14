using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Keter.Api/Features/Auth/Register/RegisterEndpoint.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Keter.Api.Features.Auth.Register;

public static class RegisterEndpoint
{
    public static void MapRegisterEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register", async (
            [FromBody] RegisterRequest request, 
            [FromServices] IMediator mediator) =>
        {
            var command = new RegisterCommand(request.Email, request.Password, request.FullName);
            var profileId = await mediator.Send(command);
            
            return Results.Ok(new { ProfileId = profileId, Message = "Registration successful." });
        })
        .WithTags("Auth")
        .AllowAnonymous(); // Anyone can access this!
    }
}