using server.Interfaces;
using server.Dtos.TaskDto;
using server.Dtos.TagDto;
using server.Dtos.UserDto;
using server.Models;

namespace server.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repo;

        public TaskService(ITaskRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> UpdateTaskPositionAsync(Guid taskId, Guid newColumnId, int newOrder, string newStatus)
        {
            var task = await _repo.GetByIdAsync(taskId);
            if (task == null) return false;

            task.ColumnId = newColumnId;
            task.Order = newOrder;      
            
            task.Status = newStatus;

            await _repo.SaveChangesAsync(); 
            return true;
        }

        public async Task<bool> UpdateTaskDetailsAsync(Guid taskId, TaskUpdateDto updateDto)
        {
            var task = await _repo.GetByIdAsync(taskId);
            if (task == null) return false;

            task.Title = updateDto.Title;
            task.Description = updateDto.Description;
            task.Status = updateDto.Status;
            task.AssignedUserId = string.IsNullOrWhiteSpace(updateDto.AssignedUserId) ? null : updateDto.AssignedUserId;

            return await _repo.UpdateTaskAsync(task, updateDto.TagIds);
        }

        public async Task<TaskReadDto?> CreateTaskAsync(TaskCreateDto dto)
        {
            int nextOrder = await _repo.GetMaxOrderInColumnAsync(dto.ColumnId) + 1;
            var newTask = new ProjectTask
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                ColumnId = dto.ColumnId,
                Order = nextOrder,
                
                Status = dto.Status,
                AssignedUserId = string.IsNullOrWhiteSpace(dto.AssignedUserId) ? null : dto.AssignedUserId, // <-- ADDED
                Deadline = dto.Deadline
            };

            var createdTask = await _repo.CreateTaskAsync(newTask, dto.TagIds);
            if (createdTask == null) return null;
            return new TaskReadDto
            {
                Id = createdTask.Id,
                Title = createdTask.Title,
                Description = createdTask.Description,
                ColumnId = createdTask.ColumnId,
                Order = createdTask.Order,
                Status = createdTask.Status,
                Deadline = createdTask.Deadline, 

                AssignedUser = createdTask.AssignedUser != null ? new UserPayloadDto 
                {
                    Id = createdTask.AssignedUser.Id,
                    UserName = createdTask.AssignedUser.UserName,
                    Email = createdTask.AssignedUser.Email,
                    ProfilePictureUrl = createdTask.AssignedUser.ProfilePictureUrl
                } : null,
                
                Tags = createdTask.Tags.Select(t => new TagReadDto 
                { 
                    Id = t.Id, 
                    Name = t.Name, 
                    ColorHex = t.ColorHex 
                }).ToList()
            };
        }
        public async Task<bool> DeleteTaskAsync(Guid id)
{
            return await _repo.DeleteTaskAsync(id);
        }
    }
}