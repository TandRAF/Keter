using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;
using System;
using System.Collections.Generic; 

namespace server.Data
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
public void Configure(EntityTypeBuilder<Project> builder)
{
    builder.HasKey(p => p.Id);
    builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
    
    builder.HasOne(p => p.Owner)
        .WithMany(u => u.OwnedProjects)
        .HasForeignKey(p => p.OwnerId) 
        .OnDelete(DeleteBehavior.Cascade);

    var projectId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    builder.HasData(new Project { 
        Id = projectId, 
        Name = "Platforma Keter", 
        Description = "Proiect pentru facultate", 
        CreatedAt = DateTime.UtcNow,
        OwnerId = "user-admin-1"
    });
    builder.HasMany(p => p.Members)
           .WithMany(u => u.MemberOfProjects)
           .UsingEntity<Dictionary<string, object>>(
               "ProjectMembers",
               j => j.HasOne<ApplicationUser>().WithMany().HasForeignKey("UserId"),
               j => j.HasOne<Project>().WithMany().HasForeignKey("ProjectId"),
               j => {
                   j.HasData(
                       new { ProjectId = projectId, UserId = "user-member-1" },
                       new { ProjectId = projectId, UserId = "user-member-2" },
                       new { ProjectId = projectId, UserId = "user-member-3" },
                       new { ProjectId = projectId, UserId = "user-member-4" }
                   );
               }
           );
}
    }
}