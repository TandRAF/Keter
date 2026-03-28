using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.TagDto;
using server.Interfaces;
using server.Models;

namespace server.Services
{
    public class TagService:ITagService
    {
        private readonly ITagRepository _repo;
        public TagService(ITagRepository repo)
        {
            _repo = repo;
        }
        public async Task<TagReadDto> CreateTagAsync(TagCreateDto dto)
        {
            var tag = new Tag
             {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                ColorHex = dto.ColorHex
            };

            var createdTag = await _repo.CreateTagAsync(tag);

            return new TagReadDto
            {
                Id = createdTag.Id,
                Name = createdTag.Name,
                ColorHex = createdTag.ColorHex
            };
        }
    }
}