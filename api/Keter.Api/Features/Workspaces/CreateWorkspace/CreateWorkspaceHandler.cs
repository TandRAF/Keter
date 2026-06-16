using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Keter.Api/Features/Workspaces/CreateWorkspace/CreateWorkspaceHandler.cs
using MediatR;
using Keter.Domain.Entities;
using Keter.Api.Infrastructure.Database;

namespace Keter.Api.Features.Workspaces.CreateWorkspace;

public class CreateWorkspaceHandler : IRequestHandler<CreateWorkspaceCommand, Guid>
{
    private readonly ApplicationDbContext _context;

    public CreateWorkspaceHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        // 1. Initialize the new Workspace
        var workspace = new Workspace
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CreatedAt = DateTime.UtcNow
        };

        // 2. Make the creator the Admin of this new workspace
        var adminMember = new WorkspaceMember
        {
            WorkspaceId = workspace.Id,
            UserId = request.UserId,
            Role = "Admin"
        };

        // 3. Save to PostgreSQL
        _context.Workspaces.Add(workspace);
        _context.WorkspaceMembers.Add(adminMember);

        await _context.SaveChangesAsync(cancellationToken);

        // 4. Return the ID so the frontend can redirect the user to their new workspace
        return workspace.Id;
    }
}