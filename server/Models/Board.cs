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
        
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }

        public List<BoardColumn> Columns { get; set; } = new();
    }
}