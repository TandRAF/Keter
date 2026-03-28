using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces;
using server.Models;

namespace server.Repositories
{
    public class ColumnRepository : IColumnRepository
    {
        private readonly ApplicationDbContext _context;

        public ColumnRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BoardColumn?> GetByIdAsync(Guid id)
        {
            return await _context.BoardColumns.FindAsync(id);
        }

        public async Task<BoardColumn> CreateAsync(BoardColumn column)
        {
            await _context.BoardColumns.AddAsync(column);
            await _context.SaveChangesAsync();
            return column;
        }

        public async Task UpdateAsync(BoardColumn column)
        {
            _context.BoardColumns.Update(column);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(BoardColumn column)
        {
            _context.BoardColumns.Remove(column);
            await _context.SaveChangesAsync();
        }
    }
}