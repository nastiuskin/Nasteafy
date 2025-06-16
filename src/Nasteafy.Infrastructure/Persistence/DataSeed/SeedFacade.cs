namespace Nasteafy.Infrastructure.Persistence.DataSeed
{
    public class SeedFacade
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            await AdminSeeder.SeedAdminAsync(serviceProvider);
            await RoleSeeder.SeedRolesAsync(serviceProvider);
        }
    }
}
