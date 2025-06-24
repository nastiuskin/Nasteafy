using Microsoft.EntityFrameworkCore;
using Nasteafy.Abstractions;
using Nasteafy.Infrastructure.Persistence.Contexts;

namespace Nasteafy.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder MapEndpoints(
          this WebApplication app,
          RouteGroupBuilder? routeGroupBuilder = null)
        {
            IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

            IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

            foreach (IEndpoint endpoint in endpoints)
            {
                endpoint.MapEndpoint(builder);
            }

            return app;
        }

        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using DatabaseContext dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            dbContext.Database.Migrate();
        }
    }
}
