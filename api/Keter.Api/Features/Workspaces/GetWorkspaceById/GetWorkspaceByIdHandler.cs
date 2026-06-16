using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Keter.Api.Infrastructure.Database;

namespace Keter.Api.Features.Workspaces.GetWorkspaceById;

public class GetWorkspaceByIdHandler : IRequestHandler<GetWorkspaceByIdQuery, WorkspaceDetailsDto?>
{
    private readonly ApplicationDbContext _context;

    public GetWorkspaceByIdHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorkspaceDetailsDto?> Handle(GetWorkspaceByIdQuery request, CancellationToken cancellationToken)
    {
       
        var workspaceDetails = await _context.WorkspaceMembers
            .AsNoTracking()
            .Where(m => m.WorkspaceId == request.WorkspaceId && m.UserId == request.UserId)
            .Select(m => new WorkspaceDetailsDto(
                m.Workspace.Id,
                m.Workspace.Name,
                m.Workspace.CreatedAt,
                m.Role
            ))
            .FirstOrDefaultAsync(cancellationToken);
            
        return workspaceDetails;
    }
}