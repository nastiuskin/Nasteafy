using Nasteafy.Domain.Entities;

namespace Nasteafy.Domain.Contracts
{
    public abstract class Artist : BaseEntity
    {
        public required string Name { get; set; }
        public string? Country { get; set; }
        public string? ImageUrl { get; set; }

        public ICollection<Album> Albums { get; set; }
        public ICollection<Track> Tracks { get; set; }
    }
}
