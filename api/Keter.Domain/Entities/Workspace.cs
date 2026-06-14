using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keter.Domain.Entities;

public class Workspace
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<WorkspaceMember> Members { get; set; } = new List<WorkspaceMember>();
}