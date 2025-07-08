namespace Nasteafy.Application.Auth.Commands.Login
{
    // Why nullable? Can you return AuthResponse with nulls? I think you return Result.Fail in case of fail, not AuthResponce with nulls. 
    public record AuthResponse(string? AccessToken, string? RefreshToken);
}
