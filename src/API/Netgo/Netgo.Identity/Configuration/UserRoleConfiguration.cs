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
                // Admin
                new IdentityUserRole<string>
                {
                    UserId = "432fb0ee-4eeb-4a7a-92d7-cf19b702669d", 
                    RoleId = "9f3383c9-12e7-429b-9378-69e88022e643" 
                },
                // User
                new IdentityUserRole<string>
                {
                    UserId = "432fb0ee-4eeb-4a7a-92d7-cf19b702669d",
                    RoleId = "175ca853-af32-46fe-99e6-adf8263ce74a"  
                }
            );
        }
    }
}
