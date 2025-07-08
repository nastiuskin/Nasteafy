using Microsoft.Extensions.DependencyInjection.Extensions;
using Nasteafy.Abstractions;
using Nasteafy.Application.Extensions;
using Nasteafy.Persistence.Database.Extensions;
using System.Reflection;

namespace Nasteafy.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddInfrastructure(builder.Configuration)
                .AddApplication()
                .AddSwagger();

            if (!builder.Environment.IsEnvironment("Testing"))
            {
                builder.Services.AddJwtAuthentication(builder.Configuration);
            }
        }

        public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
        {
            var serviceDescriptors = assembly.DefinedTypes
                .Where(type => !type.IsAbstract && !type.IsInterface && type.IsAssignableTo(typeof(IEndpoint)))
                .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
                .ToArray();

            services.TryAddEnumerable(serviceDescriptors);
            return services;
        }
    }
}