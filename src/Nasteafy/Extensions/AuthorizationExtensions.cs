namespace Nasteafy.Extensions
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin"));
            });

            return services;
        }
    }
}