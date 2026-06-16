using System;
using MediatR;

namespace Keter.Api.Features.Workspaces.AddWorkspaceMember;

public record AddWorkspaceMemberRequest(string Email, string Role);

public record AddWorkspaceMemberCommand(
    Guid WorkspaceId, 
    string Email, 
    string Role, 
    string RequesterId 
) : IRequest<bool>;