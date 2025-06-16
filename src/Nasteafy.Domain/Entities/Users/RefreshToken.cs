namespace Nasteafy.Domain.Entities.Users
{
    public class RefreshToken
    {
        public string Token { get; }
        public DateTime ExpiresAt { get; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        private RefreshToken(string token, DateTime expiresAt)
        {
            Token = token;
            ExpiresAt = expiresAt;
        }

        public static RefreshToken CreateNew(string token, TimeSpan lifetime)
        {
            return new RefreshToken(token, DateTime.UtcNow.Add(lifetime));
        }
    }
}
