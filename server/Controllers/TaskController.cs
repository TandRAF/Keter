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
            var success = await _taskService.UpdateTaskPositionAsync(id, moveDto.NewColumnId, moveDto.NewOrder);
            
            if (!success) return NotFound("Task not found.");

            return NoContent(); // 204 No Content este standardul pentru un update cu succes care nu returnează date
        }
    }
}