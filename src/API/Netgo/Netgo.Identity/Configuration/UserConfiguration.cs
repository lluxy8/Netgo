using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Netgo.Identity.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            var adminUser = new ApplicationUser
            {
                Id = "432fb0ee-4eeb-4a7a-92d7-cf19b702669d",
                FirstName = "Server",
                LastName = "Admin",
                NormalizedUserName = "ADMIN",
                ProfilePictureURL = "",
                VerifiedSeller = true,
                ContactInfo = "Telefon numaram +90 000 00 00",
                Location = "İsntanbul, Esenler",
                UserName = "admin",
                Email = "admin@netgo.com",
                NormalizedEmail = "ADMIN@NETGO.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = Guid.NewGuid().ToString("D"),
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123*");

            builder.HasData(adminUser);
        }
    }
}