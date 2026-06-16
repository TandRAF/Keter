using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Keter.Api/Features/Workspaces/CreateWorkspace/CreateWorkspaceEndpoint.cs
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Keter.Api.Features.Workspaces.CreateWorkspace;

public static class CreateWorkspaceEndpoint
{
    public static void MapCreateWorkspaceEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/workspaces", async (
            [FromBody] CreateWorkspaceRequest request,
            ClaimsPrincipal user,
            [FromServices] IMediator mediator) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            var command = new CreateWorkspaceCommand(request.Name, userId);
            var workspaceId = await mediator.Send(command);
            
            return Results.Created($"/api/workspaces/{workspaceId}", new { Id = workspaceId });
        })
        .WithTags("Workspaces")
        .RequireAuthorization();
    }
}