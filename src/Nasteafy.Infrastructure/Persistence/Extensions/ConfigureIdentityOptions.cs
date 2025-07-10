using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Nasteafy.Infrastructure.Options;

namespace Nasteafy.Infrastructure.Persistence.Extensions
{
    public class ConfigureIdentityOptions : IConfigureOptions<IdentityOptions>
    {
        private readonly IdentitySettings _settings;

        public ConfigureIdentityOptions(IOptions<IdentitySettings> options)
        {
            _settings = options.Value; 
        }

        public void Configure(IdentityOptions options)
        {
            options.Password.RequiredLength = _settings.RequiredLength;
            options.Password.RequireDigit = _settings.RequireDigit;
            options.Password.RequireNonAlphanumeric = _settings.RequireNonAlphanumeric;
            options.Password.RequireUppercase = _settings.RequireUppercase;
        }
    }
}

