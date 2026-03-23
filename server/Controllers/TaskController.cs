using Microsoft.AspNetCore.Mvc;
using server.Services;
using server.Dtos.TaskDto;
using server.Interfaces;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPut("{id}/move")]
public async Task<IActionResult> MoveTask(Guid id, [FromBody] MoveTaskDto moveDto)
{
    try 
    {
        if (moveDto == null) return BadRequest("Invalid move data.");

        var success = await _taskService.UpdateTaskPositionAsync(id, moveDto.NewColumnId, moveDto.NewOrder);
        
        if (!success) return NotFound("Task or Column not found.");

        return NoContent(); 
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error moving task: {ex.Message}");
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}
    }
}