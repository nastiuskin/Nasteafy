namespace Nasteafy.Infrastructure.Options
{
    public class IdentitySettings
    {
        public int RequiredLength { get; set; }
        public bool RequireDigit { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
        public bool RequireUppercase { get; set; }
    }
}
