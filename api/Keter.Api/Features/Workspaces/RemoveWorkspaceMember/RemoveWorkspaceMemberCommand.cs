using System;
using MediatR;

namespace Keter.Api.Features.Workspaces.RemoveWorkspaceMember;

// We need the Workspace ID, the User getting removed, and the User making the request.
public record RemoveWorkspaceMemberCommand(
    Guid WorkspaceId, 
    string TargetUserId, 
    string RequesterId
) : IRequest<bool>;