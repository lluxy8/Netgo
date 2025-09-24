using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Models;
using Netgo.Application.Models.Identity;
using Netgo.Identity.Services;
using System.Security.Claims;
using System.Text;

namespace Netgo.Identity
{
    public static class ServiceRegistry
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(
                configuration.GetSection("JwtSettings")
            );

            services.Configure<EmailSettings>(
                configuration.GetSection("EmailSettings")
            );

            services.Configure<MinioSettings>(
                configuration.GetSection("MinioSettings")
            );

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<NetgoIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddDbContext<NetgoIdentityDbContext>(o =>
                o.UseSqlServer(
                    configuration.GetConnectionString("IdentityDb"),
                    b =>
                    {
                        b.MigrationsAssembly(typeof(NetgoIdentityDbContext).Assembly.FullName);
                    }));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>{
                o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
                    };

                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey("authToken"))
                        {
                            context.Token = context.Request.Cookies["authToken"];
                        }
                        return Task.CompletedTask;
                    },

                    

                    OnTokenValidated = async context =>
                    {
                        var principal = context.Principal;
                        if (principal is null)
                        {
                            context.Fail("Unauthorized");
                            return;
                        }

                        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
                        
                        var uid = principal.FindFirstValue(CustomClaimTypes.Uid);
                        var user = await userManager.FindByIdAsync(uid!);

                        if (user == null)
                        {
                            context.Fail("Unauthorized");
                            return;
                        }

                        var securityStampFromDb = await userManager.GetSecurityStampAsync(user);
                        var securityStampFromToken = principal.FindFirstValue(CustomClaimTypes.SecurityStamp);

                        if (securityStampFromToken != securityStampFromDb)
                        {
                            context.Fail("Token is no longer valid.");
                        }
                    }                   
                };
            });

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
