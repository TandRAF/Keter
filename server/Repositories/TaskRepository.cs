using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces;
using server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace server.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectTask?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks
                .Include(t => t.Tags)
                .Include(t => t.AssignedUser)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // AM ACTUALIZAT AICI: am adăugat "string newStatus" în parametri
        public async Task<bool> UpdateTaskPositionAsync(Guid taskId, Guid newColumnId, int newOrder, string newStatus)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return false;

            task.ColumnId = newColumnId;
            task.Order = newOrder;
            task.Status = newStatus; // <-- AM ADĂUGAT ASTA CA SĂ SALVEZE STATUSUL

            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateTaskAsync(ProjectTask task, List<Guid>? tagIds)
        {
            if (tagIds != null)
            {
                task.Tags.Clear();
                var tagsToAdd = await _context.Tags.Where(t => tagIds.Contains(t.Id)).ToListAsync();
                task.Tags.AddRange(tagsToAdd);
            }

            _context.Tasks.Update(task);
            return await SaveChangesAsync();
        }

        public async Task<int> GetMaxOrderInColumnAsync(Guid columnId)
        {
            var maxOrder = await _context.Tasks
            .Where(t => t.ColumnId == columnId)
            .MaxAsync(t => (int?)t.Order) ?? -1; 
        
            return maxOrder;
        }

        public async Task<ProjectTask> CreateTaskAsync(ProjectTask task, List<Guid>? tagIds)
        {
            if (tagIds != null && tagIds.Any())
            {
                var tags = await _context.Tags.Where(t => tagIds.Contains(t.Id)).ToListAsync();
                task.Tags.AddRange(tags);
            }

            await _context.Tasks.AddAsync(task);
            await SaveChangesAsync();

            return await GetByIdAsync(task.Id); 
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}