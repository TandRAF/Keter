using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces;
using server.Models;

namespace server.Repositories
{

    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> MoveTaskAsync(Guid taskId, Guid newColumnId, int newOrder)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return false;

            // Actualizăm poziția
            task.ColumnId = newColumnId;
            task.Order = newOrder;

            // Pentru MVP, salvăm pur și simplu noua poziție. 
            // Într-o aplicație complexă, aici ai recalcula și câmpul 'Order' pentru celelalte task-uri.
            await _context.SaveChangesAsync();
            return true;
        }
    }
}