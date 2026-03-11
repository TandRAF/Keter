using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;
using System;

namespace server.Data
{
    public class BoardConfiguration : IEntityTypeConfiguration<Board>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Name).IsRequired().HasMaxLength(50);

            builder.HasMany(b => b.Tasks).WithOne(t => t.Board).HasForeignKey(t => t.BoardId).OnDelete(DeleteBehavior.Cascade);

            // SEED DATA: Legate de proiectul cu ID-ul 1111...
            builder.HasData(
                new Board { Id = Guid.Parse("22222222-2222-2222-2222-222222222221"), Name = "Backend C#", ProjectId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                new Board { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Frontend React", ProjectId = Guid.Parse("11111111-1111-1111-1111-111111111111") }
            );
        }
    }
}