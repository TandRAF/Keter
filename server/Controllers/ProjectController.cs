// Controllers/ProjectController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dtos;
using server.Dtos.ProjectDto;
using server.Interfaces;

[Authorize]
[Route("api/project")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _service;

    public ProjectController(IProjectService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectReadDto>>> GetAll() {
       var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();
        var entities = await _service.GetAllEntitiesAsync(userId);
        return Ok(entities);
    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectReadDto>> GetById(Guid id)
    {
        var entity = await _service.GetEntityByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(entity);
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ProjectReadDto>> Create([FromBody] ProjectCreateDto entityCreateDto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                 ?? User.FindFirst("sub")?.Value;

        if (userId == null) return Unauthorized();
        entityCreateDto.OwnerId = userId;
        var result = await _service.CreateEntityAsync(entityCreateDto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result); 
    }
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProjectUpdateDto updateEntity)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();
        var result = await _service.UpdateEntityAsync(id, updateEntity, userId);
    
        return result ? NoContent() : NotFound();
    }
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();
        var result = await _service.DeleteEntityAsync(id, userId);
        return result ? NoContent() : NotFound();
    }
    [Authorize]
    [HttpPost("{id}/members")]
    public async Task<IActionResult> AddMember(Guid id, [FromBody] AddMemberDto dto)
    {
        var requesterId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine($"Request for Project: {id}");
        Console.WriteLine($"Logged in User (Requester): {requesterId}");
        if (requesterId == null) return Unauthorized();

        var success = await _service.AddMemberAsync(id, dto.Username, requesterId);
    
        if (!success) return BadRequest(new { Message = "Could not add member. User not found or permission denied." });
    
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}/members/{memberId}")]
    public async Task<IActionResult> RemoveMember(Guid id, string memberId)
    {
        
        var requesterId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (requesterId == null) return Unauthorized();

        var success = await _service.RemoveMemberAsync(id, memberId, requesterId);
    
        if (!success) return BadRequest(new { Message = "Could not remove member." });

        return NoContent();
    }
}