using Nasteafy.Domain.Contracts;

namespace Nasteafy.Domain.Entities
{
    public class Genre : BaseEntity
    {
        public ICollection<Track> Tracks { get; set; }
    }
}
