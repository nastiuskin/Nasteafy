using Microsoft.EntityFrameworkCore;
using Nasteafy.Abstractions;
using Nasteafy.Infrastructure.Persistence.Contexts;

namespace Nasteafy.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder MapEndpoints(this IApplicationBuilder app)
        {
            var endpoints = app.ApplicationServices.GetRequiredService<IEnumerable<IEndpoint>>();

            app.UseEndpoints(builder =>
            {
                foreach (var endpoint in endpoints)
                {
                    endpoint.MapEndpoint(builder);
                }
            });

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
