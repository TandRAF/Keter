using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace server.Data
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string> { RoleId = "role-admin-id", UserId = "user-admin-1" },
                new IdentityUserRole<string> { RoleId = "role-user-id", UserId = "user-member-1" },
                new IdentityUserRole<string> { RoleId = "role-user-id", UserId = "user-member-2" },
                new IdentityUserRole<string> { RoleId = "role-user-id", UserId = "user-member-3" },
                new IdentityUserRole<string> { RoleId = "role-user-id", UserId = "user-member-4" }
            );
        }
    }
}