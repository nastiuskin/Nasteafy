using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Infrastructure.Persistence.DataSeed
{
    public static class RoleSeeder
    {
        private static readonly string[] Roles = Enum.GetNames<UserRole>();

        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
        }
    }
}
