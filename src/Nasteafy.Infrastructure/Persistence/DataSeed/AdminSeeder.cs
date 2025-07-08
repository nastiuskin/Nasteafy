using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Infrastructure.Persistence.DataSeed
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            const string adminEmail = "admin@nasteafy.local";
            const string adminPassword = "Admincik!";

            var roleExists = await roleManager.RoleExistsAsync("Admin");
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true 
                    //Playlists = [],
                    //UserSubscriptions = [],
                };

                var result = await userManager.CreateAsync(newAdmin, adminPassword);

                // Either use one line braces everywhere or don't use it anywhere. 
                // Generally you want to use curly braces even if there is only one line. For consistency and readability and less field for errors. 
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
