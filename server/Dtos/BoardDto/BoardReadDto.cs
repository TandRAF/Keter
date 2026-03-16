using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.ColumnDto;
namespace server.Dtos.BoardDto
{
    public class BoardReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        
        // Trimitem întreg board-ul cu tot cu coloane spre React
        public List<ColumnReadDto> Columns { get; set; } = new();
    }
}