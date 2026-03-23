using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

         public string OwnerId { get; set; } = string.Empty;
        public ApplicationUser? Owner { get; set; }

        public List<ApplicationUser> Members { get; set; } = new();

        public List<Board> Boards { get; set; } = new();
    }
}