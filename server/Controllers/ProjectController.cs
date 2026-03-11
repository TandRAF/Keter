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

    [Authorize(Roles = "Admin, User")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectReadDto>>> GetAll() {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();
        var entities = await _service.GetAllEntitiesAsync(userId);
        return Ok(entities);
    }
    [Authorize(Roles = "Admin, User")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectReadDto>> GetById(Guid id)
    {
        var entity = await _service.GetEntityByIdAsync(id);
        if (entity == null) return NotFound(); // Response 404
        return Ok(entity); // Response 200
    }
    [Authorize(Roles = "Admin, User")]
    [HttpPost]
    public async Task<ActionResult<ProjectReadDto>> Create([FromBody] ProjectCreateDto entityCreateDto)
    {
        var result = await _service.CreateEntityAsync(entityCreateDto);
        // Returnăm 201 Created și locația noii resurse
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result); 
    }
    [Authorize(Roles = "Admin, User")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProjectUpdateDto updateEntity)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();
        var result = await _service.UpdateEntityAsync(id, updateEntity, userId);
    
        return result ? NoContent() : NotFound();
    }
    [Authorize(Roles = "Admin, User")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();
        var result = await _service.DeleteEntityAsync(id, userId);
        return result ? NoContent() : NotFound();
    }
}