using MediatR;

namespace Keter.Api.Features.Workspaces.DeleteWorkspace;

// We return a boolean: true if successful, false if it wasn't found or unauthorized
public record DeleteWorkspaceCommand(Guid WorkspaceId, string UserId) : IRequest<bool>;