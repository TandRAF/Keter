// Repositories/ProjectRepository.cs
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces;
using server.Models;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Project>> GetAllByUserIdAsync(string userId) {
    return await _context.Projects
        .AsNoTracking() // Menținem viteza de citire
        .Where(p => p.OwnerId == userId) // Filtrăm proiectele care aparțin utilizatorului
        .ToListAsync();
}

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        // Căutăm elementul după ID fără a-l monitoriza pentru schimbări
        return await _context.Projects.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id); 
    }

    public async Task<Project> CreateAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync(); // Aici se generează ID-ul în DB
        return project;
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
}