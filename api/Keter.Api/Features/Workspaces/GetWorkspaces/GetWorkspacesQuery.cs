using System;
using System.Collections.Generic;
using MediatR;

namespace Keter.Api.Features.Workspaces.GetWorkspaces;

// The Query: "Get all workspaces for this specific user"
public record GetWorkspacesQuery(string UserId) : IRequest<List<WorkspaceDto>>;

// The DTO: The clean package of data we send back to the frontend
public record WorkspaceDto(Guid Id, string Name, string Role, DateTime CreatedAt);