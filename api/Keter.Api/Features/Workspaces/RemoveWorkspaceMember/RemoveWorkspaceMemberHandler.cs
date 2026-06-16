using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Keter.Api.Infrastructure.Database;

namespace Keter.Api.Features.Workspaces.RemoveWorkspaceMember;

public class RemoveWorkspaceMemberHandler : IRequestHandler<RemoveWorkspaceMemberCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public RemoveWorkspaceMemberHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(RemoveWorkspaceMemberCommand request, CancellationToken cancellationToken)
    {
        // 1. EXISTENCE CHECK: Is the target user actually in this workspace?
        var targetMembership = await _context.WorkspaceMembers
            .FirstOrDefaultAsync(wm => wm.WorkspaceId == request.WorkspaceId && wm.UserId == request.TargetUserId, cancellationToken);

        if (targetMembership == null)
        {
            throw new ArgumentException("User is not a member of this workspace.");
        }

        // 2. SECURITY CHECK: Are they leaving voluntarily, or being kicked?
        if (request.RequesterId != request.TargetUserId) 
        {
            // They are trying to kick someone else. They MUST be an Admin.
            var requesterMembership = await _context.WorkspaceMembers
                .AsNoTracking()
                .FirstOrDefaultAsync(wm => wm.WorkspaceId == request.WorkspaceId && wm.UserId == request.RequesterId, cancellationToken);

            if (requesterMembership == null || requesterMembership.Role != "Admin")
            {
                throw new UnauthorizedAccessException("Only workspace admins can remove other members.");
            }
        }

        // 3. DISASTER PREVENTION: Don't allow the last Admin to be removed/leave
        if (targetMembership.Role == "Admin")
        {
            var adminCount = await _context.WorkspaceMembers
                .CountAsync(wm => wm.WorkspaceId == request.WorkspaceId && wm.Role == "Admin", cancellationToken);

            if (adminCount <= 1)
            {
                throw new InvalidOperationException("Cannot remove the last admin. Promote someone else to Admin first.");
            }
        }

        // 4. All checks passed, remove them!
        _context.WorkspaceMembers.Remove(targetMembership);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}