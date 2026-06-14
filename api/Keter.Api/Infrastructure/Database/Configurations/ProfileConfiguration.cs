using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Keter.Api/Infrastructure/Database/Configurations/ProfileConfiguration.cs
using Keter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Keter.Api.Infrastructure.Database.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        // Table Name
        builder.ToTable("Profiles");

        // Primary Key
        builder.HasKey(p => p.Id);

        // Properties
        builder.Property(p => p.FullName).HasMaxLength(100);
        builder.Property(p => p.Bio).HasMaxLength(500);
        builder.Property(p => p.ProfilePictureUrl).HasMaxLength(255);

        // 1-to-1 Relationship: A User has one Profile
        builder.HasOne(p => p.User)
               .WithOne(u => u.Profile)
               .HasForeignKey<Profile>(p => p.UserId)
               .OnDelete(DeleteBehavior.Cascade); // If user is deleted, delete the profile
    }
}