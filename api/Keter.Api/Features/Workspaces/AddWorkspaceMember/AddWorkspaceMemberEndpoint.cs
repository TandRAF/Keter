using System;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Keter.Api.Features.Workspaces.AddWorkspaceMember;

public static class AddWorkspaceMemberEndpoint
{
    public static void MapAddWorkspaceMemberEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/workspaces/{workspaceId:guid}/members", async (
            Guid workspaceId,
            [FromBody] AddWorkspaceMemberRequest request,
            ClaimsPrincipal user,
            IMediator mediator) =>
        {
            var requesterId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(requesterId))
            {
                return Results.Unauthorized();
            }

            var command = new AddWorkspaceMemberCommand(
                workspaceId,
                request.Email,
                request.Role,
                requesterId
            );

            try
            {
                await mediator.Send(command);
                return Results.Ok(new { Message = "Member added successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Results.Forbid(); 
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = ex.Message }); 
            }
        })
        .WithTags("Workspaces")
        .RequireAuthorization();
    }
}