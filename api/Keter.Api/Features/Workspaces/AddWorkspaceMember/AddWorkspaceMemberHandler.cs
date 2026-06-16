using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Keter.Api.Infrastructure.Database;
using Keter.Domain.Entities; 
namespace Keter.Api.Features.Workspaces.AddWorkspaceMember;

public class AddWorkspaceMemberHandler : IRequestHandler<AddWorkspaceMemberCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public AddWorkspaceMemberHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(AddWorkspaceMemberCommand request, CancellationToken cancellationToken)
    {
        var requesterMembership = await _context.WorkspaceMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(wm => wm.WorkspaceId == request.WorkspaceId && wm.UserId == request.RequesterId, cancellationToken);

        if (requesterMembership == null || requesterMembership.Role != "Admin")
        {
            throw new UnauthorizedAccessException("Only workspace admins can add new members.");
        }

        var userToAdd = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (userToAdd == null)
        {
            throw new ArgumentException("A user with this email does not exist.");
        }

        var alreadyMember = await _context.WorkspaceMembers
            .AnyAsync(wm => wm.WorkspaceId == request.WorkspaceId && wm.UserId == userToAdd.Id, cancellationToken);

        if (alreadyMember)
        {
            throw new ArgumentException("This user is already a member of the workspace.");
        }

        var newMember = new WorkspaceMember
        {
            WorkspaceId = request.WorkspaceId,
            UserId = userToAdd.Id,
            Role = request.Role
        };

        _context.WorkspaceMembers.Add(newMember);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}