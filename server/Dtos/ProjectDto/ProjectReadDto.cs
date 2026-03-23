using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.UserDto;

namespace server.Dtos.ProjectDto
{
    public class ProjectReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string OwnerId { get; set; } = string.Empty;
        public UserPayloadDto Owner { get; set; } = null!;
        public List<UserPayloadDto> Members { get; set; } = new();
    }
}