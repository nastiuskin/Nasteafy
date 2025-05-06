using Nasteafy.Domain.Contracts;
namespace Nasteafy.Domain.Entities
{
    public class Band : Artist
    {
        public ICollection<SoloArtist> Members { get; set; }
    }
}
