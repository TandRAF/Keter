using System;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Keter.Api.Features.Workspaces.GetWorkspaceById;

public static class GetWorkspaceByIdEndpoint
{
    public static void MapGetWorkspaceByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/workspaces/{id:guid}", async (
            Guid id,
            ClaimsPrincipal user, 
            IMediator mediator) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            var query = new GetWorkspaceByIdQuery(id, userId);
            var workspace = await mediator.Send(query);
    
            if (workspace == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(workspace);
        })
        .WithTags("Workspaces")
        .RequireAuthorization();
    }
}