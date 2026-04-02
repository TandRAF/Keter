using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace server.Dtos.BoardDto
{
    public class BoardCreateDto
    {
        [Required]
        public Guid ProjectId { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}