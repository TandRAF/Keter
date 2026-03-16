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

        public async Task<Board?> GetBoardWithDetailsAsync(Guid boardId)
        {
            // Aducem ierarhia completă și o ordonăm din start
            return await _context.Boards
                .Include(b => b.Columns.OrderBy(c => c.Order)) // Ordonăm coloanele
                    .ThenInclude(c => c.Tasks.OrderBy(t => t.Order)) // Ordonăm task-urile în coloană
                        .ThenInclude(t => t.Tags) // Aducem tag-urile fiecărui task
                .FirstOrDefaultAsync(b => b.Id == boardId);
        }
    }
}