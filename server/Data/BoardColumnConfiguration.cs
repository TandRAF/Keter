using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;
using System;

namespace server.Data
{
    public class BoardColumnConfiguration : IEntityTypeConfiguration<BoardColumn>
    {
        public void Configure(EntityTypeBuilder<BoardColumn> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Title).IsRequired().HasMaxLength(50);
            builder.HasMany(c => c.Tasks)
                   .WithOne(t => t.Column)
                   .HasForeignKey(t => t.ColumnId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new BoardColumn { Id = Guid.Parse("44444444-4444-4444-4444-444444444411"), Title = "To Do", Order = 0, BoardId = Guid.Parse("22222222-2222-2222-2222-222222222221") },
                new BoardColumn { Id = Guid.Parse("44444444-4444-4444-4444-444444444412"), Title = "In Progress", Order = 1, BoardId = Guid.Parse("22222222-2222-2222-2222-222222222221") },
                new BoardColumn { Id = Guid.Parse("44444444-4444-4444-4444-444444444413"), Title = "Done", Order = 2, BoardId = Guid.Parse("22222222-2222-2222-2222-222222222221") }
            );

            builder.HasData(
                new BoardColumn { Id = Guid.Parse("44444444-4444-4444-4444-444444444421"), Title = "To Do", Order = 0, BoardId = Guid.Parse("22222222-2222-2222-2222-222222222222") },
                new BoardColumn { Id = Guid.Parse("44444444-4444-4444-4444-444444444422"), Title = "In Progress", Order = 1, BoardId = Guid.Parse("22222222-2222-2222-2222-222222222222") },
                new BoardColumn { Id = Guid.Parse("44444444-4444-4444-4444-444444444423"), Title = "Done", Order = 2, BoardId = Guid.Parse("22222222-2222-2222-2222-222222222222") }
            );
        }
    }
}