using Nasteafy.Domain.Entities.Users;
using System.Security.Claims;

namespace Nasteafy.Application.Common.Abstractions.Auth
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(List<Claim> claims);
        RefreshToken GenerateRefreshToken();
    }
}
