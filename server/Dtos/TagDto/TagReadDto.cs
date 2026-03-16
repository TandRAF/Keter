using System;

namespace server.Dtos.TagDto
{
    public class TagReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ColorHex { get; set; } = string.Empty;
    }
}