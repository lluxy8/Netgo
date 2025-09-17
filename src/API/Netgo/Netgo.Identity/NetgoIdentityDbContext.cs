using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Netgo.Identity.Configuration;

namespace Netgo.Identity
{
    public class NetgoIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public NetgoIdentityDbContext(DbContextOptions<NetgoIdentityDbContext> options)
            : base(options) 
        {        
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
        }
    }
}
