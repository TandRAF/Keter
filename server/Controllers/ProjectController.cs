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
    public async Task<ActionResult<IEnumerable<ProjectReadDto>>> GetAll()
    {
        var entities = await _service.GetAllEntitiesAsync();
        return Ok(entities); // Response 200 Ok
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectReadDto>> GetById(Guid id)
    {
        var entity = await _service.GetEntityByIdAsync(id);
        if (entity == null) return NotFound(); // Response 404
        return Ok(entity); // Response 200
    }

    [HttpPost]
    public async Task<ActionResult<ProjectReadDto>> Create([FromBody] ProjectCreateDto entityCreateDto)
    {
        var result = await _service.CreateEntityAsync(entityCreateDto);
        // Returnăm 201 Created și locația noii resurse
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result); 
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProjectUpdateDto updateEntity)
    {
        var result = await _service.UpdateEntityAsync(id, updateEntity);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteEntityAsync(id);
        return result ? NoContent() : NotFound();
    }
}