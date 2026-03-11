using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllByUserIdAsync(string userId);
        Task<Project?> GetByIdAsync(Guid id);

        Task<Project> CreateAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);

    }
}