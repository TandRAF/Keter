using MediatR;
using Microsoft.EntityFrameworkCore;
using Keter.Api.Infrastructure.Database;

namespace Keter.Api.Features.Workspaces.DeleteWorkspace;

public class DeleteWorkspaceHandler : IRequestHandler<DeleteWorkspaceCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public DeleteWorkspaceHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var member = await _context.WorkspaceMembers
            .FirstOrDefaultAsync(m => m.WorkspaceId == request.WorkspaceId && m.UserId == request.UserId, cancellationToken);

        if (member == null || member.Role != "Admin")
        {
            return false; 
        }

        var workspace = await _context.Workspaces
            .FirstOrDefaultAsync(w => w.Id == request.WorkspaceId, cancellationToken);

        if (workspace == null)
        {
            return false;
        }

        _context.Workspaces.Remove(workspace);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}