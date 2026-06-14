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
            [FromBody] CreateWorkspaceRequest request, // Acum primim doar Numele
            ClaimsPrincipal user,                      // <-- .NET injectează utilizatorul curent aici!
            [FromServices] IMediator mediator) =>
        {
            // 1. Extragem ID-ul utilizatorului din token-ul JWT (NameIdentifier este standardul pt ID)
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // 2. Măsură de siguranță: dacă nu găsim ID-ul, îi dăm reject.
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            // 3. Construim comanda completă și o trimitem la Handler
            var command = new CreateWorkspaceCommand(request.Name, userId);
            var workspaceId = await mediator.Send(command);
            
            return Results.Created($"/api/workspaces/{workspaceId}", new { Id = workspaceId });
        })
        .WithTags("Workspaces")
        .RequireAuthorization(); // <-- ACEASTA ESTE CRITICĂ. Obligă userul să aibă token JWT.
    }
}