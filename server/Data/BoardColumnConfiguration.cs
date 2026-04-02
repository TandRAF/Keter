using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;
using System;
using System.Collections.Generic;

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

            var columns = new List<BoardColumn>();
            var boardIds = new[]
            {
                "22222222-2222-2222-2222-222222222221",
                "22222222-2222-2222-2222-222222222222",
                "22222222-2222-2222-2222-222222222223",
                "22222222-2222-2222-2222-222222222224",
                "22222222-2222-2222-2222-222222222225",
                "22222222-2222-2222-2222-222222222226",
                "22222222-2222-2222-2222-222222222227"
            };

            int colIndex = 11;
            foreach (var bId in boardIds)
            {
                columns.Add(new BoardColumn { Id = Guid.Parse($"44444444-4444-4444-4444-4444444444{colIndex++}"), Title = "To Do", Order = 0, BoardId = Guid.Parse(bId) });
                columns.Add(new BoardColumn { Id = Guid.Parse($"44444444-4444-4444-4444-4444444444{colIndex++}"), Title = "In Progress", Order = 1, BoardId = Guid.Parse(bId) });
                columns.Add(new BoardColumn { Id = Guid.Parse($"44444444-4444-4444-4444-4444444444{colIndex++}"), Title = "Done", Order = 2, BoardId = Guid.Parse(bId) });
            }

            builder.HasData(columns);
        }
    }
}