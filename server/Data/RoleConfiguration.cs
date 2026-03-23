using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace server.Data
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole { Id = "role-user-id", Name = "User", NormalizedName = "USER" },
                new IdentityRole { Id = "role-admin-id", Name = "Admin", NormalizedName = "ADMIN" }
            );
        }
    }
}