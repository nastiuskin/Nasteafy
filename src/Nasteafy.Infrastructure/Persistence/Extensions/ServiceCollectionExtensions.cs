using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Data.Repositories;
using Nasteafy.Domain.Entities.Users;
using Nasteafy.Infrastructure.Database.Repositories;
using Nasteafy.Infrastructure.Options;
using Nasteafy.Infrastructure.Persistence.Contexts;
using Nasteafy.Infrastructure.Persistence.DataSeed;
using Nasteafy.Infrastructure.Persistence.Repositories;
using Nasteafy.Infrastructure.Services;

namespace Nasteafy.Persistence.Database.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDatabase(configuration)
                    .AddRepositores(configuration)
                    .AddMinio(configuration)
                    .AddUnitOfWork()
                    .AddIdentity()
                    .AddServices();

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));

            return services;
        }

        private static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole<Guid>>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            return services;
        }
        private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.SectionName));

            services.AddSingleton(sp =>
            {
                var options = sp.GetRequiredService<IOptions<MinioOptions>>().Value;

                return new Minio.MinioClient()
                    .WithEndpoint(options.Endpoint)
                    .WithCredentials(options.AccessKey, options.SecretKey)
                    .Build();
            });

            services.AddScoped<IFileStorageService, MinioStorageService>();

            return services;
        }

        private static IServiceCollection AddRepositores(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITrackRepository, TrackRepository>();
            services.AddScoped<IPlaylistRepository, PlaylistRepository>();
            services.AddScoped<IArtistRepository, ArtistRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IAlbumRepository, AlbumRepository>();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
            services.AddScoped<IFileStorageService, MinioStorageService>();
            services.AddScoped<IResultLogger, ResultLogger>();
            return services;
        }
    }
}

