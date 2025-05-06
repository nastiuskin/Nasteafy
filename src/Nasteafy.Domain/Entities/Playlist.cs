using Nasteafy.Domain.Contracts;

namespace Nasteafy.Domain.Entities
{
    public class Playlist : MediaEntity
    {
        public required Guid UserId { get; set; }
        public bool IsPublic { get; set; }

        public required User User { get; set; }
        public ICollection<Track> Tracks { get; set; }
    }
}
