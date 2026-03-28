using System;
using System.Collections.Generic;

namespace server.Dtos.TaskDto
{
    public class TaskCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid ColumnId { get; set; } 
        
        public string Status { get; set; } = string.Empty; 

        public string? AssignedUserId { get; set; }
        public DateTime? Deadline { get; set; }
        
        public List<Guid>? TagIds { get; set; } 
    }
}