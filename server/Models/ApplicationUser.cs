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

        // Proiectele pe care le-a creat (Admin)
        public List<Project> OwnedProjects { get; set; } = new();

        // Task-urile la care este asignat (Member)
        public List<ProjectTask> AssignedTasks { get; set; } = new();
    }
}