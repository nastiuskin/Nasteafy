using System.Security.Claims;

namespace Nasteafy.Application.Abstractions.Auth
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(List<Claim> claims);
        string GenerateRefreshToken();
    }
}
