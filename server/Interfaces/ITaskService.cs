using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Interfaces
{
    public interface ITaskService
    {
        Task<bool> UpdateTaskPositionAsync(Guid taskId, Guid newColumnId, int newOrder);
    }
}