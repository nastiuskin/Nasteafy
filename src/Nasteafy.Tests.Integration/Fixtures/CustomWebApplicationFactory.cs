using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Extensions;
using Nasteafy.Infrastructure.Persistence.Contexts;
using System.Data.Common;
using System.Net.Http.Headers;

namespace Nasteafy.Tests.Integration.Fixtures;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptorsToRemove = services
                .Where(d =>
                    d.ServiceType == typeof(DbContextOptions<DatabaseContext>) ||
                    d.ServiceType == typeof(DbContext) ||
                    d.ImplementationType == typeof(DatabaseContext) ||
                    d.ServiceType == typeof(DbConnection) ||
                    d.ServiceType == typeof(IDbContextFactory<DatabaseContext>) ||
                    (d.ServiceType?.FullName?.Contains("EntityFramework") ?? false))
                .ToList();

            foreach (var descriptor in descriptorsToRemove)
                services.Remove(descriptor);

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            services.AddEndpoints(typeof(Program).Assembly);

            services.AddScoped<IFileStorageService, TestFileStorage>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "TestAuth";
                options.DefaultChallengeScheme = "TestAuth";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestAuth", _ => { });


            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            //});           

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            db.Database.EnsureCreated();

            SeedData(db);
        });

        builder.Configure(app =>
        {
            app.UseRouting(); 
            app.UseGlobalExceptionHandling();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapEndpoints();
        });
    }

    private void SeedData(DatabaseContext db)
    {
        db.Artists.AddRange(
            new Artist
            {
                Id = Guid.NewGuid(),
                Name = "Test Artist 1",
            },
            new Artist
            {
                Id = Guid.NewGuid(),
                Name = "Test Artist 2",
            }
        );
        db.SaveChanges();
    }

    public HttpClient CreateAuthorizedClient()
    {
        var client = this.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestAuth");

        return client;
    }
}
