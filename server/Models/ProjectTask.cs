using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
   public class ProjectTask
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid ColumnId { get; set; }
        public BoardColumn? Column { get; set; }
        public int Order { get; set; } 
        public string? AssignedUserId { get; set; }
        public ApplicationUser? AssignedUser { get; set; }
        public List<Tag> Tags { get; set; } = new();
    }
}