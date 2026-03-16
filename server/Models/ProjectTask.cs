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
        
        // Statusul este acum definit de Coloana de care aparține
        public Guid ColumnId { get; set; }
        public BoardColumn? Column { get; set; }
        public int Order { get; set; } // Ordinea (indexul) în interiorul coloanei pentru Drag & Drop

        public string? AssignedUserId { get; set; }
        public ApplicationUser? AssignedUser { get; set; }

        // Relația cu Tag-urile (Categoriile)
        public List<Tag> Tags { get; set; } = new();
    }
}