using System;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Keter.Api.Features.Workspaces.GetWorkspaceMembers;

public static class GetWorkspaceMembersEndpoint
{
    public static void MapGetWorkspaceMembersEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/workspaces/{workspaceId:guid}/members", async (
            Guid workspaceId,
            ClaimsPrincipal user, 
            IMediator mediator) =>
        {
            // Extract the user ID from the JWT token
            var requesterId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(requesterId))
            {
                return Results.Unauthorized();
            }

            var query = new GetWorkspaceMembersQuery(workspaceId, requesterId);
            var members = await mediator.Send(query);
            
            // If the list is empty, it means they have no members OR they aren't allowed to see them
            if (!members.Any())
            {
                return Results.Forbid(); 
            }

            return Results.Ok(members);
        })
        .WithTags("Workspaces")
        .RequireAuthorization();
    }
}