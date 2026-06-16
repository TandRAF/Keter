using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Keter.Api.Features.Workspaces.DeleteWorkspace;

public static class DeleteWorkspaceEndpoint
{
    public static void MapDeleteWorkspaceEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/workspaces/{id:guid}", async (
            Guid id, 
            ClaimsPrincipal user, 
            [FromServices] IMediator mediator) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            var command = new DeleteWorkspaceCommand(id, userId);
            var success = await mediator.Send(command);

            if (!success)
            {
                return Results.Forbid(); 
            }
            return Results.NoContent();
        })
        .WithTags("Workspaces")
        .RequireAuthorization();
    }
}