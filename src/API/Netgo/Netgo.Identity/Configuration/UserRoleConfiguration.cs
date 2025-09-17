using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Netgo.Identity.Configuration
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    UserId = "432fb0ee-4eeb-4a7a-92d7-cf19b702669d", // admin user
                    RoleId = "9f3383c9-12e7-429b-9378-69e88022e643"  // Admin role
                }
            );
        }
    }
}
