using server.Interfaces;
using server.Repositories;

namespace server.Services
{

    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepo;

        public TaskService(ITaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
        }

        public async Task<bool> UpdateTaskPositionAsync(Guid taskId, Guid newColumnId, int newOrder)
        {
            return await _taskRepo.MoveTaskAsync(taskId, newColumnId, newOrder);
        }
    }
}