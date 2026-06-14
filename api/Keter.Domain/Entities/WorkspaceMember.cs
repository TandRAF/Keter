using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Keter.Domain/Entities/WorkspaceMember.cs
namespace Keter.Domain.Entities;

public class WorkspaceMember
{
    public Guid WorkspaceId { get; set; }
    public string UserId { get; set; } = string.Empty;
    
    // e.g., "Admin", "Member", "Viewer"
    public string Role { get; set; } = string.Empty; 

    // Navigation Properties
    public Workspace Workspace { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}