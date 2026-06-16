using System;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Keter.Api.Features.Workspaces.RemoveWorkspaceMember;

public static class RemoveWorkspaceMemberEndpoint
{
    public static void MapRemoveWorkspaceMemberEndpoint(this IEndpointRouteBuilder app)
    {
        // Notice the targetUserId is in the route path!
        app.MapDelete("/api/workspaces/{workspaceId:guid}/members/{targetUserId}", async (
            Guid workspaceId,
            string targetUserId, 
            ClaimsPrincipal user,
            IMediator mediator) =>
        {
            var requesterId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(requesterId))
            {
                return Results.Unauthorized();
            }

            var command = new RemoveWorkspaceMemberCommand(workspaceId, targetUserId, requesterId);

            try
            {
                await mediator.Send(command);
                return Results.NoContent(); // 204: Successfully deleted
            }
            catch (ArgumentException ex)
            {
                return Results.NotFound(new { Error = ex.Message }); // 404: Target wasn't in the workspace
            }
            catch (UnauthorizedAccessException ex)
            {
                return Results.Forbid(); // 403: Not an admin
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { Error = ex.Message }); // 400: Tried to delete the last admin
            }
        })
        .WithTags("Workspaces")
        .RequireAuthorization();
    }
}