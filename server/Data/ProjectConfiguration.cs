using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;
using System;

namespace server.Data
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);

            builder.HasMany(p => p.Boards).WithOne(b => b.Project).HasForeignKey(b => b.ProjectId).OnDelete(DeleteBehavior.Cascade);

            // SEED DATA: ID fix pentru proiect
            builder.HasData(
                new Project 
                { 
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), 
                    Name = "Platforma Keter", 
                    Description = "Proiect pentru facultate", 
                    CreatedAt = DateTime.UtcNow,
                    OwnerId = "user-admin-1" // Legat de admin!
                }
            );
        }
    }
}