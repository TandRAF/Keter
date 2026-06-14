using Keter.Api.Infrastructure.Database;
using Keter.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Keter.Api.Infrastructure.Database.Seeding;

public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public DatabaseSeeder(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task SeedAsync()
{
    // Căutăm direct dacă admin-ul există deja
    var adminEmail = "admin@keter.local";
    var adminUser = await _userManager.FindByEmailAsync(adminEmail);

    // 1. Dacă nu există, îl creăm
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };
        await _userManager.CreateAsync(adminUser, "Password123!");
    }

    // 2. Verificăm separat dacă există Workspace-uri
    if (!await _context.Workspaces.AnyAsync())
    {
        var workspace = new Workspace
        {
            Id = Guid.NewGuid(),
            Name = "Keter Alpha Workspace",
            CreatedAt = DateTime.UtcNow
        };

        var workspaceMember = new WorkspaceMember
        {
            WorkspaceId = workspace.Id,
            UserId = adminUser.Id, // Folosim ID-ul adminului, indiferent dacă abia l-am creat sau exista deja
            Role = "Admin"
        };

        _context.Workspaces.Add(workspace);
        _context.WorkspaceMembers.Add(workspaceMember);
        
        await _context.SaveChangesAsync();
    }
    }
}