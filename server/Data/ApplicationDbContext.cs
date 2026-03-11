using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data
{
    // Clasa trebuie să moștenească IdentityDbContext pentru a include tabelele de User/Login
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Definirea seturilor de date (Tabelele)
        public DbSet<Project> Projects { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // FOARTE IMPORTANT: Se apelează baza pentru a configura tabelele de Identity
            base.OnModelCreating(builder);
            
            // Această linie aplică automat configurațiile din UserConfiguration, ProjectConfiguration etc.
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}