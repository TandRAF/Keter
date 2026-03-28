using System;
using System.Collections.Generic;
using server.Dtos.TagDto;
using server.Dtos.UserDto; // Add this

namespace server.Dtos.TaskDto
{
    public class TaskReadDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid ColumnId { get; set; } 
        public int Order { get; set; }
        public DateTime? Deadline { get; set; }
        public string Status { get; set; } = string.Empty;
        public UserPayloadDto? AssignedUser { get; set; } 
        public List<TagReadDto> Tags { get; set; } = new();
    }
}