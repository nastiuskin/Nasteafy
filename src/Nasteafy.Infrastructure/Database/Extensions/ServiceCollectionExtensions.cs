using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nasteafy.Application.Abstractions;
using Nasteafy.Application.Abstractions.Data;
using Nasteafy.Application.Abstractions.Data.Repositories;
using Nasteafy.Infrastructure.Database;
using Nasteafy.Infrastructure.Database.Options;
using Nasteafy.Infrastructure.Database.Repositories;

namespace Nasteafy.Persistence.Database.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDatabase(configuration)
                    .AddRepositores(configuration);

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));

            services.AddScoped<IUnitOfWork, DatabaseContext>();
            return services;
        }

        private static IServiceCollection AddRepositores(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITrackRepository, TrackRepository>();
            services.AddScoped<IPlaylistRepository, PlaylistRepository>();
            services.AddScoped<IArtistRepository, ArtistRepository>();

            return services;
        }
    }
}
