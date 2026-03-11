using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class Board
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        // Foreign Key către Proiect
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }

        public List<ProjectTask> Tasks { get; set; } = new();
    }
}