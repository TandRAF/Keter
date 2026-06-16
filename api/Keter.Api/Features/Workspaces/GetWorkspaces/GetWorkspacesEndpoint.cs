using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Keter.Api.Features.Workspaces.GetWorkspaces;

public static class GetWorkspacesEndpoint
{
    public static void MapGetWorkspacesEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/workspaces", async (
            ClaimsPrincipal user, 
            IMediator mediator) =>
        {
            // Extract the user ID securely from the JWT token
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            var query = new GetWorkspacesQuery(userId);
            var workspaces = await mediator.Send(query);
            
            return Results.Ok(workspaces);
        })
        .WithTags("Workspaces")
        .RequireAuthorization(); // Critical: Forces JWT validation
    }
}