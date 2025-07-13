namespace Nasteafy.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void AddUserSecretsConfiguration(this WebApplicationBuilder builder)
        {
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Program>();
        }
    }
}