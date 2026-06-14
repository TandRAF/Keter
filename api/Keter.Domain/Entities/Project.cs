using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Keter.Domain/Entities/Project.cs
namespace Keter.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public Guid WorkspaceId { get; set; }
    public string Name { get; set; } = string.Empty;
    
    // The short code for Jira-style ticket prefixes (e.g., "KTR")
    public string Key { get; set; } = string.Empty; 
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation Property
    public Workspace Workspace { get; set; } = null!;
}