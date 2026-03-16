using System;
using System.Collections.Generic;
using server.Dtos.TagDto;

namespace server.Dtos.TaskDto
{
    public class TaskReadDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        
        public Guid ColumnId { get; set; } 
        public int Order { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? AssignedUserId { get; set; }
        public List<TagReadDto> Tags { get; set; } = new();
    }
}