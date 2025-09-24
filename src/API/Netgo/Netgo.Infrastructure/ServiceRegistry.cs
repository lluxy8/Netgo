using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netgo.Application.Contracts.Infrastructure;
using Netgo.Application.Models;
using Netgo.Application.Models.Identity;
using Netgo.Infrastructure.Services;
using System.Threading.Tasks;

namespace Netgo.Infrastructure
{
    public static class ServiceRegistry
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IFileService, S3FileService> ();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
