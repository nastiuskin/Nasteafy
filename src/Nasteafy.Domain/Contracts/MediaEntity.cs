namespace Nasteafy.Domain.Contracts
{
    public abstract class MediaEntity : BaseEntity
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required DateTime ReleaseDate { get; set; }
        public string? CoverUrl { get; set; }
    }
}
