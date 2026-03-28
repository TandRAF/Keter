using server.Models;

namespace server.Interfaces
{
    public interface IColumnRepository
    {
        Task<BoardColumn?> GetByIdAsync(Guid id);
        Task<BoardColumn> CreateAsync(BoardColumn column);
        Task UpdateAsync(BoardColumn column);
        Task DeleteAsync(BoardColumn column);
    }
}