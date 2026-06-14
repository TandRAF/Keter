// Keter.Api/Infrastructure/Database/Configurations/WorkspaceConfiguration.cs
using Keter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Keter.Api.Infrastructure.Database.Configurations;

public class WorkspaceConfiguration : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        // 1. Numele tabelului
        builder.ToTable("Workspaces");

        // 2. Cheia primară
        builder.HasKey(w => w.Id);

        // 3. Proprietăți
        builder.Property(w => w.Name)
               .IsRequired()
               .HasMaxLength(100); // Evităm "text" infinit în baza de date

        builder.Property(w => w.CreatedAt)
               .IsRequired();

        // 4. Relația 1-la-Mulți cu WorkspaceMember
        builder.HasMany(w => w.Members)
               .WithOne(wm => wm.Workspace)
               .HasForeignKey(wm => wm.WorkspaceId)
               .OnDelete(DeleteBehavior.Cascade); // Dacă ștergi Workspace-ul, se șterg și legăturile cu membrii

        // 5. Relația 1-la-Mulți cu Project
        builder.HasMany(w => w.Projects)
               .WithOne(p => p.Workspace)
               .HasForeignKey(p => p.WorkspaceId)
               .OnDelete(DeleteBehavior.Cascade); // Dacă ștergi Workspace-ul, se șterg și proiectele
    }
}