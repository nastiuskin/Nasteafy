namespace Nasteafy.Endpoints.Users.Update
{
    public sealed record UpdateUserProfileRequest
    {
        public string? Email { get; init; }
        public string? UserName { get; init; }
        public IFormFile? AvatarFile { get; init; }
    }
}
