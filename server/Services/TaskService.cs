using server.Interfaces;
using server.Repositories;

namespace server.Services
{

    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repo;

        public TaskService(ITaskRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> UpdateTaskPositionAsync(Guid taskId, Guid newColumnId, int newOrder)
        {

        var task = await _repo.GetByIdAsync(taskId);
        if (task == null) return false;

        task.ColumnId = newColumnId;
        task.Order = newOrder;      

        await _repo.SaveChangesAsync(); 
    
        return true;
        }
    }
}