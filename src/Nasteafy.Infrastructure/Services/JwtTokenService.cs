using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nasteafy.Application.Abstractions.Auth;
using Nasteafy.Domain.Entities.Users;
using Nasteafy.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Nasteafy.Infrastructure.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions jwtOptions;

        public JwtTokenService(IOptions<JwtOptions> jwtOptions)
        {
            this.jwtOptions = jwtOptions.Value;
        }
        public string GenerateAccessToken(List<Claim> claims)
        {
            var signinCredentials = new SigningCredentials(jwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                 issuer: jwtOptions.Issuer,
                 audience: jwtOptions.Audience,
                 claims: claims,
                 expires: DateTime.UtcNow.AddMinutes(jwtOptions.AccessTokenExpirationMinutes),
                 signingCredentials: signinCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            var encodedToken = tokenHandler.WriteToken(jwtSecurityToken);
            return encodedToken;
        }

        public RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            var refreshToken = RefreshToken.CreateNew(
                Convert.ToBase64String(randomNumber),
                TimeSpan.FromDays(jwtOptions.RefreshTokenExpirationDays));
            return refreshToken;
        }
    }
}
