using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Keter.Api.Infrastructure.Database;

namespace Keter.Api.Features.Workspaces.GetWorkspaceMembers;

public class GetWorkspaceMembersHandler : IRequestHandler<GetWorkspaceMembersQuery, List<WorkspaceMemberDto>>
{
    private readonly ApplicationDbContext _context;

    public GetWorkspaceMembersHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<WorkspaceMemberDto>> Handle(GetWorkspaceMembersQuery request, CancellationToken cancellationToken)
    {
        // 1. Security Check: Is the person asking actually a member of this workspace?
        var isMember = await _context.WorkspaceMembers
            .AnyAsync(wm => wm.WorkspaceId == request.WorkspaceId && wm.UserId == request.RequesterId, cancellationToken);

        if (!isMember)
        {
            // If they aren't in the workspace, return an empty list (or throw an exception)
            return new List<WorkspaceMemberDto>();
        }

        // 2. Fetch and join the data
        var members = await (from wm in _context.WorkspaceMembers
                             join p in _context.Profiles on wm.UserId equals p.UserId
                             where wm.WorkspaceId == request.WorkspaceId
                             orderby wm.Role // Optional: Sorts Admin before Member
                             select new WorkspaceMemberDto(
                                 wm.UserId,
                                 p.FullName, // This acts as your nickname
                                 p.ProfilePictureUrl,
                                 wm.Role
                             )).ToListAsync(cancellationToken);

        return members;
    }
}