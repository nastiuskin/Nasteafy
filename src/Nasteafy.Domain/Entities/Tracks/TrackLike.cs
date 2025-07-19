using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Domain.Entities.Tracks
{
    public class TrackLike
    {
        public Guid TrackId { get; set; }
        public Track Track { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
