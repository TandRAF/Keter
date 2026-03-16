using System;
using System.Collections.Generic;

namespace server.Models
{
    public class BoardColumn
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Order { get; set; } 
        public Guid BoardId { get; set; }
        public Board? Board { get; set; }

        public List<ProjectTask> Tasks { get; set; } = new();
    }
}