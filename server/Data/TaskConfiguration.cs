using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;
using System;

namespace server.Data
{
    public class TaskConfiguration : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
            builder.Property(t => t.Status).HasConversion<string>();

            // SEED DATA
            builder.HasData(
                new ProjectTask { 
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333331"), 
                    Title = "Configurare Docker", 
                    Status = ProjectTaskStatus.Done, 
                    BoardId = Guid.Parse("22222222-2222-2222-2222-222222222221"), // Backend Board
                    AssignedUserId = "user-member-1" 
                },
                new ProjectTask { 
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333332"), 
                    Title = "Creare DbContext", 
                    Status = ProjectTaskStatus.InProgress, 
                    BoardId = Guid.Parse("22222222-2222-2222-2222-222222222221"), // Backend Board
                    AssignedUserId = "user-member-2" 
                },
                new ProjectTask { 
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), 
                    Title = "Design UI in SCSS", 
                    Status = ProjectTaskStatus.Todo, 
                    BoardId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Frontend Board
                    AssignedUserId = "user-member-3" 
                },
                new ProjectTask { 
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333334"), 
                    Title = "Integrare Axios", 
                    Status = ProjectTaskStatus.Todo, 
                    BoardId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Frontend Board
                    AssignedUserId = "user-member-4" 
                }
            );
        }
    }
}