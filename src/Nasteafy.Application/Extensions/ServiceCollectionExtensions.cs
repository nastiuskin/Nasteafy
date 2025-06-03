using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nasteafy.Application.Behaviors;
using Nasteafy.Application.Exceptions;
using System.Reflection;

namespace Nasteafy.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddExceptionHandler<GlobalExceptionHandler>();

            return services;
        }
    }
}
