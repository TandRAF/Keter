using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public enum ProjectTaskStatus
    {
        Todo,
        InProgress,
        Done
    }

    public class ProjectTask
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ProjectTaskStatus Status { get; set; } = ProjectTaskStatus.Todo;

        public string? AssignedUserId { get; set; }
        public ApplicationUser? AssignedUser { get; set; }

        public Guid BoardId { get; set; }
        public Board? Board { get; set; }
    }
}