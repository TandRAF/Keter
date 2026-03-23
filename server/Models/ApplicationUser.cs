using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace server.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? FullName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public List<Project> OwnedProjects { get; set; } = new();
        public List<ProjectTask> AssignedTasks { get; set; } = new();
        public List<Project> MemberOfProjects { get; set; } = new();
    }
}