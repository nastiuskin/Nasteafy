using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Nasteafy.Infrastructure.Options;

namespace Nasteafy.Infrastructure.Persistence.Extensions
{
    public class ConfigureIdentityOptions(IdentitySettings settings) : IConfigureOptions<IdentityOptions>
    {
        public void Configure(IdentityOptions options)
        {
            options.Password.RequiredLength = settings.Password.RequiredLength;
            options.Password.RequireDigit = settings.Password.RequireDigit;
            options.Password.RequireNonAlphanumeric = settings.Password.RequireNonAlphanumeric;
            options.Password.RequireUppercase = settings.Password.RequireUppercase;
        }
    }
}
