using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Application.Auth.Commands.Login
{
    public record AuthResponse(string AccessToken, RefreshToken RefreshToken);
}
