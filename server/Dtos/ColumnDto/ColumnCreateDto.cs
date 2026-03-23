using System;

namespace server.Dtos.ColumnDto
{
    public class ColumnCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public int Order { get; set; }
        public Guid BoardId { get; set; }
    }
}