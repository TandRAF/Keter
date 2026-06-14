// Keter.Api/Infrastructure/Database/Configurations/ProjectConfiguration.cs
using Keter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Keter.Api.Infrastructure.Database.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(100);

        // Limităm Key-ul (gen 'KTR' pentru task-uri Jira-style)
        builder.Property(p => p.Key)
               .IsRequired()
               .HasMaxLength(10); 
               
        builder.Property(p => p.Description)
               .HasMaxLength(500);

        // Legătura cu Workspace-ul
        builder.HasOne(p => p.Workspace)
               .WithMany(w => w.Projects)
               .HasForeignKey(p => p.WorkspaceId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}