using Microsoft.AspNetCore.Mvc;
using server.Services;
using server.Dtos.TaskDto;
using server.Interfaces;
using System;
using System.Threading.Tasks;

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
                var success = await _taskService.UpdateTaskPositionAsync(id, moveDto.NewColumnId, moveDto.NewOrder, moveDto.NewStatus);
                
                if (!success) return NotFound("Task or Column not found.");

                return NoContent(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error moving task: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null) return BadRequest("Invalid task data.");

                var success = await _taskService.UpdateTaskDetailsAsync(id, updateDto);

                if (!success) return NotFound("Task not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating task: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                
                var createdTask = await _taskService.CreateTaskAsync(dto);
                
                if (createdTask == null) return BadRequest("Could not create task.");
                
                return Ok(createdTask);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating task: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}