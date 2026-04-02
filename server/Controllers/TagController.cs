using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.TagDto;
using server.Interfaces;
using server.Models;

[ApiController]
[Route("api/tag")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [Authorize]
    [HttpGet("{id}/tags")]
    public async Task<IActionResult> GetProjectTags(Guid id, [FromServices] ITagRepository tagRepo, [FromServices] IMapper mapper)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var tags = await tagRepo.GetTagsByProjectIdAsync(id);

        var tagDtos = mapper.Map<List<TagReadDto>>(tags);
        return Ok(tagDtos);
    }

    [Authorize]
    [HttpPost("{id}/tags")]
    public async Task<IActionResult> CreateProjectTag(Guid id, [FromBody] TagCreateDto dto, [FromServices] ITagRepository tagRepo, [FromServices] IMapper mapper)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var newTag = new Tag
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            ColorHex = dto.ColorHex,
            ProjectId = id 
        };

        var createdTag = await tagRepo.CreateTagAsync(newTag);
        return Ok(mapper.Map<TagReadDto>(createdTag));
    }
}