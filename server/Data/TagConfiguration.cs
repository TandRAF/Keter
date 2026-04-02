using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;
using System;
using System.Collections.Generic;

namespace server.Data
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(30);

            var tags = new List<Tag>();
            var projectIds = new[] 
            {
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Guid.Parse("11111111-1111-1111-1111-111111111112"),
                Guid.Parse("11111111-1111-1111-1111-111111111113"),
                Guid.Parse("11111111-1111-1111-1111-111111111114")
            };

            int tagIndex = 1;
            foreach (var projectId in projectIds)
            {
                tags.Add(new Tag { Id = Guid.Parse($"55555555-5555-5555-5555-5555555555{tagIndex:D2}"), Name = "Important", ColorHex = "#E27893", ProjectId = projectId }); tagIndex++;
                tags.Add(new Tag { Id = Guid.Parse($"55555555-5555-5555-5555-5555555555{tagIndex:D2}"), Name = "Programming", ColorHex = "#734FCF", ProjectId = projectId }); tagIndex++;
                tags.Add(new Tag { Id = Guid.Parse($"55555555-5555-5555-5555-5555555555{tagIndex:D2}"), Name = "UI/UX", ColorHex = "#EDCF8E", ProjectId = projectId }); tagIndex++;
                tags.Add(new Tag { Id = Guid.Parse($"55555555-5555-5555-5555-5555555555{tagIndex:D2}"), Name = "Social Media", ColorHex = "#C28CAE", ProjectId = projectId }); tagIndex++;
                tags.Add(new Tag { Id = Guid.Parse($"55555555-5555-5555-5555-5555555555{tagIndex:D2}"), Name = "Optional", ColorHex = "#D9D9D9", ProjectId = projectId }); tagIndex++;
            }

            builder.HasData(tags);
        }
    }
}