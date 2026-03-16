using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Interfaces
{
    public interface ITaskRepository
    {
        Task<bool> MoveTaskAsync(Guid taskId, Guid newColumnId, int newOrder);
    }
}