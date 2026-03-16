using System;
using System.Collections.Generic;
using server.Dtos.TaskDto;

namespace server.Dtos.ColumnDto
{
    public class ColumnReadDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Order { get; set; }
        public List<TaskReadDto> Tasks { get; set; } = new();
    }
}