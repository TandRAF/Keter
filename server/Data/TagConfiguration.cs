using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;
using System;

namespace server.Data
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(30);

            builder.HasData(
                new Tag { Id = Guid.Parse("55555555-5555-5555-5555-555555555551"), Name = "Feature", ColorHex = "#3b82f6" }, // Blue
                new Tag { Id = Guid.Parse("55555555-5555-5555-5555-555555555552"), Name = "Bug", ColorHex = "#ef4444" }, // Red
                new Tag { Id = Guid.Parse("55555555-5555-5555-5555-555555555553"), Name = "DevOps", ColorHex = "#10b981" } // Green
            );
        }
    }
}