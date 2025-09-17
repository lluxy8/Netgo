using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netgo.Application.Contracts.Infrastructure;
using Netgo.Application.Models.Identity;
using Netgo.Infrastructure.Services;

namespace Netgo.Infrastructure
{
    public static class ServiceRegistry
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            /*
            var minioEndpoint = configuration["MINIO_ENDPOINT"];
            var minioAccessKey = configuration["MINIO_ACCESS_KEY"];
            var minioSecretKey = configuration["MINIO_SECRET_KEY"];
            var bucketName = "netgo-files";
                
            services.AddSingleton<IAmazonS3>(sp =>
            {
                return new AmazonS3Client(
                    minioAccessKey,
                    minioSecretKey,
                    new AmazonS3Config
                    {
                        ServiceURL = $"http://{minioEndpoint}",
                        ForcePathStyle = true 
                    });
            });
            */

            services.AddTransient<IFileService, S3FileService> ();

            /*
            services.AddScoped<IFileService>(sp =>
                new S3FileService(sp.GetRequiredService<IAmazonS3>(), bucketName));
            */
            services.AddScoped<IEmailService, EmailService>();


            return services;
        }
    }
}
