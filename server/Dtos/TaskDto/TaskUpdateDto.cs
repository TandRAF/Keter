using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.TaskDto
{
    public class TaskUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? AssignedUserId { get; set; }
        public List<Guid>? TagIds { get; set; }
    }
}