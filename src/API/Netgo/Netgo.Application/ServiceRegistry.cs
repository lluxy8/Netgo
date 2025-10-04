using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Netgo.Application.Behaviors;
using System.Reflection;

namespace Netgo.Application
{
    public static class ServiceRegistry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPipelineBehavior<,>));

            return services;
        }
    }
}
