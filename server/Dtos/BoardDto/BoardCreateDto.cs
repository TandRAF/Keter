using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.BoardDto
{
    public class BoardCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
    }
}