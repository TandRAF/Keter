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
            builder.HasMany(b => b.Columns)
                   .WithOne(c => c.Board)
                   .HasForeignKey(c => c.BoardId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Board { Id = Guid.Parse("22222222-2222-2222-2222-222222222221"), Name = "Backend C#", ProjectId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                new Board { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Frontend React", ProjectId = Guid.Parse("11111111-1111-1111-1111-111111111111") },

                new Board { Id = Guid.Parse("22222222-2222-2222-2222-222222222223"), Name = "Dezvoltare Magazin", ProjectId = Guid.Parse("11111111-1111-1111-1111-111111111112") },
                new Board { Id = Guid.Parse("22222222-2222-2222-2222-222222222224"), Name = "Marketing", ProjectId = Guid.Parse("11111111-1111-1111-1111-111111111112") },

                new Board { Id = Guid.Parse("22222222-2222-2222-2222-222222222225"), Name = "iOS App", ProjectId = Guid.Parse("11111111-1111-1111-1111-111111111113") },
                new Board { Id = Guid.Parse("22222222-2222-2222-2222-222222222226"), Name = "Android App", ProjectId = Guid.Parse("11111111-1111-1111-1111-111111111113") },

                new Board { Id = Guid.Parse("22222222-2222-2222-2222-222222222227"), Name = "Implementare ERP", ProjectId = Guid.Parse("11111111-1111-1111-1111-111111111114") }
            );
        }
    }
}