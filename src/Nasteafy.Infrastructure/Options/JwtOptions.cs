using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Nasteafy.Infrastructure.Options
{
    public class JwtOptions
    {
        public const string SectionName = "MinioOptions";
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
            => new(Encoding.UTF8.GetBytes(SecretKey));
    }
}
