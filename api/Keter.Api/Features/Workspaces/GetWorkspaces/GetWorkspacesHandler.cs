using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Keter.Api.Infrastructure.Database;

namespace Keter.Api.Features.Workspaces.GetWorkspaces;

public class GetWorkspacesHandler : IRequestHandler<GetWorkspacesQuery, List<WorkspaceDto>>
{
    private readonly ApplicationDbContext _context;

    public GetWorkspacesHandler(ApplicationDbContext context)
    {
        _context = context;
    }

   public async Task<List<WorkspaceDto>> Handle(GetWorkspacesQuery request, CancellationToken cancellationToken)
    {
        var workspaces = await _context.WorkspaceMembers
            .AsNoTracking()
            .Where(member => member.UserId == request.UserId)
            .Include(member => member.Workspace)
            // 🌟 FIX: Sort the database entities FIRST
            .OrderByDescending(member => member.Workspace.CreatedAt) 
            // 🌟 THEN map it to the C# object
            .Select(member => new WorkspaceDto(
                member.Workspace.Id,
                member.Workspace.Name,
                member.Role,
                member.Workspace.CreatedAt
            ))
            .ToListAsync(cancellationToken);

        return workspaces;
    }
}