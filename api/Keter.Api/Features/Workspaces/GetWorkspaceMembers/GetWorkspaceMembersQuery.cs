using System;
using System.Collections.Generic;
using MediatR;

namespace Keter.Api.Features.Workspaces.GetWorkspaceMembers;

// The Query: "Get all members for Workspace X"
// We also pass the RequesterId to ensure they are allowed to see this data!
public record GetWorkspaceMembersQuery(Guid WorkspaceId, string RequesterId) : IRequest<List<WorkspaceMemberDto>>;

// The DTO: The clean package for the React frontend
public record WorkspaceMemberDto(
    string UserId, 
    string Nickname, 
    string? ProfileImageUrl, 
    string Role
);