using System;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces
{
    public interface ITaskRepository
    {
        Task<ProjectTask?> GetByIdAsync(Guid id);
        Task<bool> UpdateTaskPositionAsync(Guid taskId, Guid newColumnId, int newOrder);
        Task<bool> SaveChangesAsync(); 
    }
}