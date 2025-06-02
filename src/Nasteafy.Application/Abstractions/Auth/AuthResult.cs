namespace Nasteafy.Application.Abstractions.Auth
{
    public class AuthResult
    {
        public bool IsSuccess { get; init; }
        public string? AccessToken { get; init; }
        public string? RefreshToken { get; init; }
        public string? ErrorMessage { get; init; }

        public static AuthResult Success(string accessToken, string refreshToken) =>
            new() { IsSuccess = true, AccessToken = accessToken, RefreshToken = refreshToken };

        public static AuthResult Failure(string message) =>
            new() { IsSuccess = false, ErrorMessage = message };
    }
}
