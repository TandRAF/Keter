using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;
using System;
using System.Collections.Generic;

namespace server.Data
{
    public class TaskConfiguration : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
            
            builder.HasMany(t => t.Tags)
                   .WithMany(tag => tag.Tasks)
                   // We must configure the hidden join table to seed the relationships
.UsingEntity<Dictionary<string, object>>(
   "ProjectTaskTag",
   j => j.HasOne<Tag>().WithMany().HasForeignKey("TagsId"),
   j => j.HasOne<ProjectTask>().WithMany().HasForeignKey("TasksId"),
   j => 
   {
       j.HasData(
           // Task 1 gets Tag "Important" and "Programming" (Project 1 tags)
           new { TasksId = Guid.Parse("33333333-3333-3333-3333-333333333301"), TagsId = Guid.Parse("55555555-5555-5555-5555-555555555501") },
           new { TasksId = Guid.Parse("33333333-3333-3333-3333-333333333301"), TagsId = Guid.Parse("55555555-5555-5555-5555-555555555502") },
           
           // Task 4 gets Tag "Important" (Project 1 tags)
           new { TasksId = Guid.Parse("33333333-3333-3333-3333-333333333304"), TagsId = Guid.Parse("55555555-5555-5555-5555-555555555501") },
           
           // Task 7 gets Tag "UI/UX" (Project 1 tags)
           new { TasksId = Guid.Parse("33333333-3333-3333-3333-333333333307"), TagsId = Guid.Parse("55555555-5555-5555-5555-555555555503") },
           
           // Task 15 gets Tag "Social Media" (Project 4 tags)
           new { TasksId = Guid.Parse("33333333-3333-3333-3333-333333333315"), TagsId = Guid.Parse("55555555-5555-5555-5555-555555555519") },

           // Task 18 gets Tag "Programming" and "Optional" (Project 3 tags)
           new { TasksId = Guid.Parse("33333333-3333-3333-3333-333333333318"), TagsId = Guid.Parse("55555555-5555-5555-5555-555555555512") },
           new { TasksId = Guid.Parse("33333333-3333-3333-3333-333333333318"), TagsId = Guid.Parse("55555555-5555-5555-5555-555555555515") }
       );
   }
);

            builder.HasData(
                // Board 1 (6)
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333301"), Title = "Implementare JWT Token", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444411"), Order = 0, Status = "To Do", AssignedUserId = "user-member-1" },
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333302"), Title = "Creare endpoint Login", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444411"), Order = 1, Status = "To Do", AssignedUserId = "user-member-2" },
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333303"), Title = "Securizare rute API", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444411"), Order = 2, Status = "To Do", AssignedUserId = "user-admin-1" },
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333304"), Title = "Creare DbContext", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444412"), Order = 0, Status = "In Progress", AssignedUserId = "user-member-3" },
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333305"), Title = "Migrari baza de date", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444412"), Order = 1, Status = "In Progress", AssignedUserId = "user-member-4" },
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333306"), Title = "Configurare Docker", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444413"), Order = 0, Status = "Done", AssignedUserId = "user-member-1" },

                // Board 2 (6)
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333307"), Title = "Design UI in SCSS", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444414"), Order = 0, Status = "To Do", AssignedUserId = "user-member-3" },
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333308"), Title = "Integrare Axios", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444414"), Order = 1, Status = "To Do", AssignedUserId = "user-member-4" },
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333309"), Title = "Componenta BoardCard", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444415"), Order = 0, Status = "In Progress", AssignedUserId = "user-admin-1" },
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333310"), Title = "Meniu de navigare", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444415"), Order = 1, Status = "In Progress", AssignedUserId = "user-member-1" },
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333311"), Title = "Grafic statistic (CSS Grid)", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444415"), Order = 2, Status = "In Progress", AssignedUserId = "user-member-2" },
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333312"), Title = "Setup initial Vite", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444416"), Order = 0, Status = "Done", AssignedUserId = "user-member-3" },

                // Board 3 (2)
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333313"), Title = "Integrare Stripe pentru plati", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444417"), Order = 0, Status = "To Do", AssignedUserId = "user-member-1" },
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333314"), Title = "Design Pagina de Produs", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444419"), Order = 0, Status = "Done", AssignedUserId = "user-member-3" },

                // Board 4 (2)
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333315"), Title = "Setup Campanie Facebook Ads", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444420"), Order = 0, Status = "To Do", AssignedUserId = "user-member-4" },
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333316"), Title = "Design bannere promovare", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444421"), Order = 0, Status = "In Progress", AssignedUserId = "user-member-2" },

                // Board 5 (2)
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333317"), Title = "Integrare Apple Login", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444423"), Order = 0, Status = "To Do", AssignedUserId = "user-member-3" },
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333318"), Title = "Setup initial proiect Swift", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444425"), Order = 0, Status = "Done", AssignedUserId = "user-admin-1" },

                // Board 6 (1)
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333319"), Title = "Push notifications cu Firebase", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444427"), Order = 0, Status = "In Progress", AssignedUserId = "user-member-1" },

                // Board 7 (1)
                new ProjectTask { Id = Guid.Parse("33333333-3333-3333-3333-333333333320"), Title = "Dezvoltare Modul HR", ColumnId = Guid.Parse("44444444-4444-4444-4444-444444444429"), Order = 0, Status = "To Do", AssignedUserId = "user-member-4" }
            );
        }
    }
}