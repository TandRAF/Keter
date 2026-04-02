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

            var p1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var p2 = Guid.Parse("11111111-1111-1111-1111-111111111112");
            var p3 = Guid.Parse("11111111-1111-1111-1111-111111111113");
            var p4 = Guid.Parse("11111111-1111-1111-1111-111111111114");

            builder.HasData(
                new Project { Id = p1, Name = "Platforma Keter", Description = "Proiect pentru facultate", CreatedAt = DateTime.UtcNow, OwnerId = "user-admin-1" },
                new Project { Id = p2, Name = "Magazin Online", Description = "E-commerce cu React și C#", CreatedAt = DateTime.UtcNow, OwnerId = "user-admin-1" },
                new Project { Id = p3, Name = "Aplicație Mobilă", Description = "Aplicație de fitness", CreatedAt = DateTime.UtcNow, OwnerId = "user-admin-1" },
                new Project { Id = p4, Name = "Sistem Gestiune", Description = "ERP pentru firmă locală", CreatedAt = DateTime.UtcNow, OwnerId = "user-admin-1" }
            );

            builder.HasMany(p => p.Members)
                   .WithMany(u => u.MemberOfProjects)
                   .UsingEntity<Dictionary<string, object>>(
                       "ProjectMembers",
                       j => j.HasOne<ApplicationUser>().WithMany().HasForeignKey("UserId"),
                       j => j.HasOne<Project>().WithMany().HasForeignKey("ProjectId"),
                       j => {
                           j.HasData(
                               new { ProjectId = p1, UserId = "user-member-1" },
                               new { ProjectId = p1, UserId = "user-member-2" },
                               new { ProjectId = p1, UserId = "user-member-3" },
                               new { ProjectId = p1, UserId = "user-member-4" },

                               new { ProjectId = p2, UserId = "user-member-1" },
                               new { ProjectId = p2, UserId = "user-member-3" },

                               new { ProjectId = p3, UserId = "user-member-2" },
                               new { ProjectId = p3, UserId = "user-member-4" },

                               new { ProjectId = p4, UserId = "user-member-1" }
                           );
                       }
                   );
        }
    }
}