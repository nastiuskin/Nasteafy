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
                    .AddRepositories(Assembly.GetExecutingAssembly())
                    .AddMinio(configuration)
                    .AddIdentity(configuration)
                    .AddServices();

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));
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
            var repositoryTypes = assembly.GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface && type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IGenericRepository<>)));

            var nonBaseRepos = repositoryTypes.Where(t => t != typeof(GenericRepository<>));

            foreach (var repositoryType in nonBaseRepos)
            {
                var interfaces = repositoryType.GetInterfaces()
                    .Where(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IGenericRepository<>))
                    .ToList();

                if (interfaces.Count != 1)
                {
                    throw new InvalidOperationException($"Repository '{repositoryType.Name}' must implement only one interface that implements IGenericRepository<T>.");
                }

                services.AddScoped(interfaces[0], repositoryType);
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

