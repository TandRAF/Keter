// Repositories/ProjectRepository.cs
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces;
using server.Models;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;
    public ProjectRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Project>> GetAllByUserIdAsync(string userId)
    {
        return await _context.Projects
            .Include(p => p.Owner)
            .Include(p => p.Members)
            .Where(p => p.OwnerId == userId || p.Members.Any(m => m.Id == userId))
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await _context.Projects
            .Include(p => p.Owner)
            .Include(p => p.Members)
            .FirstOrDefaultAsync(e => e.Id == id); 
    }

   public async Task UpdateAsync(Project project)
{
    _context.Projects.Update(project); 
    await _context.SaveChangesAsync();
}

    public async Task DeleteAsync(Project project)
    {
        _context.Remove(project);
        await _context.SaveChangesAsync();
    }
    public async Task<Project> CreateAsync(Project project) 
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}