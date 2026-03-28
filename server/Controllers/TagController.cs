using Microsoft.AspNetCore.Mvc;
using server.Dtos.TagDto;
using server.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] TagCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var createdTag = await _tagService.CreateTagAsync(dto);
        return Ok(createdTag);
    }
}