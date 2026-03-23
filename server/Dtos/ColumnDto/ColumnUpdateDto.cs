using System;

namespace server.Dtos.ColumnDto
{

    public class ColumnUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}