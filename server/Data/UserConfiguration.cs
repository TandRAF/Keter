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
            // Mapăm explicit către tabelul standard de Identity
            builder.ToTable("AspNetUsers");

            // Relațiile
            builder.HasMany(u => u.OwnedProjects)
                   .WithOne(p => p.Owner)
                   .HasForeignKey(p => p.OwnerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.AssignedTasks)
                   .WithOne(t => t.AssignedUser)
                   .HasForeignKey(t => t.AssignedUserId)
                   .OnDelete(DeleteBehavior.SetNull);

            // SEED DATA: Creăm un hasher pentru parole
            var hasher = new PasswordHasher<ApplicationUser>();

            // 1. Adminul - working_raccoon
            var admin = new ApplicationUser { Id = "user-admin-1", UserName = "working_raccoon", NormalizedUserName = "WORKING_RACCOON", Email = "working@keter.ro", NormalizedEmail = "WORKING@KETER.RO", SecurityStamp = Guid.NewGuid().ToString() };
            admin.PasswordHash = hasher.HashPassword(admin, "ParolaGrea123!");

            // 2. Membru - money_raccoon
            var member1 = new ApplicationUser { Id = "user-member-1", UserName = "money_raccoon", NormalizedUserName = "MONEY_RACCOON", Email = "money@keter.ro", NormalizedEmail = "MONEY@KETER.RO", SecurityStamp = Guid.NewGuid().ToString() };
            member1.PasswordHash = hasher.HashPassword(member1, "ParolaGrea123!");

            // 3. Membru - cool_raccoon
            var member2 = new ApplicationUser { Id = "user-member-2", UserName = "cool_raccoon", NormalizedUserName = "COOL_RACCOON", Email = "cool@keter.ro", NormalizedEmail = "COOL@KETER.RO", SecurityStamp = Guid.NewGuid().ToString() };
            member2.PasswordHash = hasher.HashPassword(member2, "ParolaGrea123!");

            // 4. Membru - bath_raccoon
            var member3 = new ApplicationUser { Id = "user-member-3", UserName = "bath_raccoon", NormalizedUserName = "BATH_RACCOON", Email = "bath@keter.ro", NormalizedEmail = "BATH@KETER.RO", SecurityStamp = Guid.NewGuid().ToString() };
            member3.PasswordHash = hasher.HashPassword(member3, "ParolaGrea123!");

            // 5. Membru - banana_raccoon
            var member4 = new ApplicationUser { Id = "user-member-4", UserName = "banana_raccoon", NormalizedUserName = "BANANA_RACCOON", Email = "banana@keter.ro", NormalizedEmail = "BANANA@KETER.RO", SecurityStamp = Guid.NewGuid().ToString() };
            member4.PasswordHash = hasher.HashPassword(member4, "ParolaGrea123!");

            // Adăugăm toți enoții în baza de date
            builder.HasData(admin, member1, member2, member3, member4);
        }
    }
}