using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Keter.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Keter.Api.Infrastructure.Database;

// Notice we inherit from IdentityDbContext to support the Identity tables!
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) { }

    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Workspace> Workspaces { get; set; }
    public DbSet<WorkspaceMember> WorkspaceMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // 1. Always call the base method first when using IdentityDbContext!
        base.OnModelCreating(builder);

        // 2. The Magic Line: Automatically applies all IEntityTypeConfiguration classes
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}