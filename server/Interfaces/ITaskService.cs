using System;
using System.Threading.Tasks;
using server.Dtos.TaskDto;

namespace server.Interfaces
{
    public interface ITaskService
    {
        Task<bool> UpdateTaskPositionAsync(Guid taskId, Guid newColumnId, int newOrder, string newStatus);
        Task<bool> UpdateTaskDetailsAsync(Guid taskId, TaskUpdateDto updateDto);
        Task<TaskReadDto?> CreateTaskAsync(TaskCreateDto dto);
    }
}