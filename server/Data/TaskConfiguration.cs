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
            builder.HasMany(t => t.Tags)
                   .WithMany(tag => tag.Tasks);
            builder.HasData(
                new ProjectTask { 
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333331"), 
                    Title = "Configurare Docker", 
                    ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444413"),
                    Order = 0,
                    Status = "Done",
                    AssignedUserId = "user-member-1" ,
                },
                new ProjectTask { 
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333332"), 
                    Title = "Creare DbContext", 
                    ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444412"),
                    Order = 0,
                    Status = "In Progress",
                    AssignedUserId = "user-member-2" 
                },
                new ProjectTask { 
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), 
                    Title = "Design UI in SCSS", 
                    ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444421"),
                    Order = 0,
                    Status = "To Do",
                    AssignedUserId = "user-member-3" 
                },
                new ProjectTask { 
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333334"), 
                    Title = "Integrare Axios", 
                    ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444421"), 
                    Order = 1,
                    Status = "To Do",
                    AssignedUserId = "user-member-4" 
                }
            );
        }
    }
}