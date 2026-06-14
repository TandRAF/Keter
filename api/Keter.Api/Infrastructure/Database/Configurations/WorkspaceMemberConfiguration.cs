// Keter.Api/Infrastructure/Database/Configurations/WorkspaceMemberConfiguration.cs
using Keter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Keter.Api.Infrastructure.Database.Configurations;

public class WorkspaceMemberConfiguration : IEntityTypeConfiguration<WorkspaceMember>
{
    public void Configure(EntityTypeBuilder<WorkspaceMember> builder)
    {
        builder.ToTable("WorkspaceMembers");
        builder.HasKey(wm => new { wm.WorkspaceId, wm.UserId });
        builder.Property(wm => wm.Role).HasMaxLength(20).IsRequired();

        // Acum tipurile se potrivesc perfect!
        builder.HasOne(wm => wm.User)
               .WithMany(u => u.WorkspaceMembers) // <-- Am schimbat aici
               .HasForeignKey(wm => wm.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(wm => wm.Workspace)
               .WithMany(w => w.Members)
               .HasForeignKey(wm => wm.WorkspaceId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}