using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace Keter.Domain.Entities;

// Inheriting from IdentityUser gives us Id, Email, PasswordHash, etc. for free!
public class ApplicationUser : IdentityUser
{
    // Navigation Property
    public Profile Profile { get; set; } = null!;
    public ICollection<WorkspaceMember> WorkspaceMembers { get; set; } = new List<WorkspaceMember>();
}