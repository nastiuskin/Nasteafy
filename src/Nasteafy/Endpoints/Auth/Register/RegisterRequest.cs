namespace Nasteafy.Endpoints.Auth.Register
{
    public sealed record RegisterRequest
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}
