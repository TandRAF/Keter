using System;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces
{
    public interface ITaskRepository
    {
        Task<ProjectTask?> GetByIdAsync(Guid id);
       Task<bool> UpdateTaskPositionAsync(Guid taskId, Guid newColumnId, int newOrder, string newStatus);
        Task<bool> UpdateTaskAsync(ProjectTask task, List<Guid>? tagIds);
        Task<bool> SaveChangesAsync(); 
        Task<int> GetMaxOrderInColumnAsync(Guid columnId);
        Task<ProjectTask> CreateTaskAsync(ProjectTask task, List<Guid>? tagIds);
        Task<bool> DeleteTaskAsync(Guid id);
        
    }
}