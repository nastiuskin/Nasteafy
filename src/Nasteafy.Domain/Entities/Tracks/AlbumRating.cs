using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Domain.Entities.Tracks
{
    public class AlbumRating
    {
        public Guid AlbumId { get; set; }
        public Album Album { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
        public int Rating { get; set; }
    }
}
