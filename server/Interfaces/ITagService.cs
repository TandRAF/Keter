using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.TagDto;

namespace server.Interfaces
{
    public interface ITagService
    {
        Task<TagReadDto> CreateTagAsync(TagCreateDto dto);
    }
}