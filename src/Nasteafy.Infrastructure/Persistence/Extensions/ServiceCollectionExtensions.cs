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
using Nasteafy.Application.Common.Abstractions.Helpers;
using Nasteafy.Domain.Entities.Users;
using Nasteafy.Infrastructure.Database.Repositories;
using Nasteafy.Infrastructure.Options;
using Nasteafy.Infrastructure.Persistence.Contexts;
using Nasteafy.Infrastructure.Persistence.Extensions;
using Nasteafy.Infrastructure.Persistence.Repositories;
using Nasteafy.Infrastructure.Services;
using System;
using System.Reflection;

namespace Nasteafy.Persistence.Database.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDatabase(configuration)
                    .AddRepositories(typeof(TrackRepository).Assembly)
                    .AddUnitOfWork()
                    .AddMinio(configuration)
                    .AddIdentity(configuration)
                    .AddServices();

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));

            return services;
        }

        private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        private static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IdentitySettings>(configuration.GetSection(nameof(IdentitySettings)));

            services.AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IConfigureOptions<IdentityOptions>, ConfigureIdentityOptions>();

            return services;
        }

        public static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MinioOptions>(configuration.GetSection(nameof(MinioOptions)));

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

        public static IServiceCollection AddRepositories(this IServiceCollection services, Assembly assembly)
        {
            var allTypes = assembly.GetTypes();

            var repoTypes = allTypes
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .ToList();

            foreach (var impl in repoTypes)
            {
                var interfaces = impl.GetInterfaces()
                    .Where(i => i.IsInterface && i != typeof(IGenericRepository<>) &&
                                i.GetInterfaces().Any(ii =>
                                    ii.IsGenericType && ii.GetGenericTypeDefinition() == typeof(IGenericRepository<>)))
                    .ToList();

                foreach (var iface in interfaces)
                {
                    services.AddScoped(iface, impl);
                }
            }

            return services;
        }


        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
            services.AddScoped<IFileStorageService, MinioStorageService>();
            services.AddScoped<IResultLogger, ResultLogger>();
            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<ISignInService, SignInService>();
            services.AddScoped<IClaimService, ClaimService>();
            return services;
        }
    }
}

