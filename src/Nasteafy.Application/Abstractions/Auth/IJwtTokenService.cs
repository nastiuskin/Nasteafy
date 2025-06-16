using Nasteafy.Domain.Entities.Users;
using System.Security.Claims;

namespace Nasteafy.Application.Abstractions.Auth
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(List<Claim> claims);
        RefreshToken GenerateRefreshToken();
    }
}
