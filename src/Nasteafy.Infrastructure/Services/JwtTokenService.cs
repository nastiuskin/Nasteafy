using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Domain.Entities.Users;
using Nasteafy.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
            var token = Guid.NewGuid().ToString();

            var refreshToken = RefreshToken.CreateNew(token, TimeSpan.FromDays(jwtOptions.RefreshTokenExpirationDays));
            return refreshToken;
        }
    }
}
