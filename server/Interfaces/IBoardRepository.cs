using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces
{
    public interface IBoardRepository
    {
        Task<List<Board>> GetAllByProjectIdAsync(Guid projectId);
        Task<Board?> GetByIdAsync(Guid id);
        Task<Board> CreateAsync(Board board);
        Task UpdateAsync(Board board);
        Task DeleteAsync(Board board);
    }
}