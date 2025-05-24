using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Abstractions
{
    public interface ITrackRepository : IBaseRepository<Track>
    {
        IQueryable<Track> GetByArtistId(Guid artistId, CancellationToken ct);
        IQueryable<Track> GetByAlbumId(Guid albumId, CancellationToken ct);
        IQueryable<Track> GetByPlaylistId(Guid playlistId, CancellationToken ct);
        IQueryable<Track> GetByName(string name, CancellationToken ct);
    }
}
