using System;
using System.Collections.Generic;

namespace server.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ColorHex { get; set; } = "#FFFFFF"; 
        public List<ProjectTask> Tasks { get; set; } = new();
    }
}