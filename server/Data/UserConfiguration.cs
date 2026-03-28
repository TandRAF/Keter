using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Data
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("AspNetUsers");

            builder.HasMany(u => u.OwnedProjects)
                   .WithOne(p => p.Owner)
                   .HasForeignKey(p => p.OwnerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.AssignedTasks)
                   .WithOne(t => t.AssignedUser)
                   .HasForeignKey(t => t.AssignedUserId)
                   .OnDelete(DeleteBehavior.SetNull);

            var hasher = new PasswordHasher<ApplicationUser>();

            var admin = new ApplicationUser { Id = "user-admin-1", UserName = "working_raccoon", NormalizedUserName = "WORKING_RACCOON", Email = "working@keter.ro", NormalizedEmail = "WORKING@KETER.RO", SecurityStamp = Guid.NewGuid().ToString(), ProfilePictureUrl = "/../avatars/WorkingRaccoon.png" };
            admin.PasswordHash = hasher.HashPassword(admin, "ParolaGrea123!");

            var member1 = new ApplicationUser { Id = "user-member-1", UserName = "money_raccoon", NormalizedUserName = "MONEY_RACCOON", Email = "money@keter.ro", NormalizedEmail = "MONEY@KETER.RO", SecurityStamp = Guid.NewGuid().ToString(), ProfilePictureUrl = "/../avatars/CardRaccoon.png" };
            member1.PasswordHash = hasher.HashPassword(member1, "ParolaGrea123!");

            var member2 = new ApplicationUser { Id = "user-member-2", UserName = "cool_raccoon", NormalizedUserName = "COOL_RACCOON", Email = "cool@keter.ro", NormalizedEmail = "COOL@KETER.RO", SecurityStamp = Guid.NewGuid().ToString(), ProfilePictureUrl = "/../avatars/coolRaccoon.png" };
            member2.PasswordHash = hasher.HashPassword(member2, "ParolaGrea123!");

            var member3 = new ApplicationUser { Id = "user-member-3", UserName = "bath_raccoon", NormalizedUserName = "BATH_RACCOON", Email = "bath@keter.ro", NormalizedEmail = "BATH@KETER.RO", SecurityStamp = Guid.NewGuid().ToString(), ProfilePictureUrl = "/../avatars/BathRaccoon.png" };
            member3.PasswordHash = hasher.HashPassword(member3, "ParolaGrea123!");

            var member4 = new ApplicationUser { Id = "user-member-4", UserName = "banana_raccoon", NormalizedUserName = "BANANA_RACCOON", Email = "banana@keter.ro", NormalizedEmail = "BANANA@KETER.RO", SecurityStamp = Guid.NewGuid().ToString(), ProfilePictureUrl = "/../avatars/BananaRaccoon.png" };
            member4.PasswordHash = hasher.HashPassword(member4, "ParolaGrea123!");

            builder.HasData(admin, member1, member2, member3, member4);
        }
    }
}