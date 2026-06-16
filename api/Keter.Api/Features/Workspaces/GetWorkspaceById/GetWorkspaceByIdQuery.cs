using System;
using MediatR;

namespace Keter.Api.Features.Workspaces.GetWorkspaceById;

// The Query: We pass the WorkspaceId they want, AND their UserId to prove they are allowed to see it.
public record GetWorkspaceByIdQuery(Guid WorkspaceId, string UserId) : IRequest<WorkspaceDetailsDto?>;

// The DTO: What the frontend needs to render the specific workspace dashboard
public record WorkspaceDetailsDto(
    Guid Id, 
    string Name, 
    DateTime CreatedAt, 
    string MyRole // Knowing their role helps the React UI hide/show admin buttons!
);