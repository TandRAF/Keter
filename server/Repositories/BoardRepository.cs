using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces;
using server.Models;

namespace server.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly ApplicationDbContext _context;

        public BoardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Board>> GetAllByProjectIdAsync(Guid projectId)
        {
            return await _context.Boards
                .Where(b => b.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<Board?> GetByIdAsync(Guid id)
        {
            // Aici aducem tot "arborele" board-ului (Coloane -> Task-uri -> Tag-uri)
            return await _context.Boards
                .Include(b => b.Columns.OrderBy(c => c.Order)) // Ordonăm coloanele cum trebuie
                    .ThenInclude(c => c.Tasks.OrderBy(t => t.Order))
                        .ThenInclude(t => t.Tags)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Board> CreateAsync(Board board)
        {
            await _context.Boards.AddAsync(board);
            await _context.SaveChangesAsync();
            return board;
        }

        public async Task UpdateAsync(Board board)
        {
            _context.Boards.Update(board);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Board board)
        {
            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();
        }

        public Task<Board?> GetBoardWithDetailsAsync(Guid boardId)
        {
            throw new NotImplementedException();
        }
    }
}