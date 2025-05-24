using Nasteafy.Application.Abstractions;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Persistence.Repositories
{
    public class TrackRepository : ITrackRepository
    {
        private readonly List<Track> _tracks = new();

        public Task AddAsync(Track entity, CancellationToken ct)
        {
            _tracks.Add(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var track = _tracks.FirstOrDefault(t => t.Id == id);
            if (track is not null) 
                _tracks.Remove(track);

            return Task.CompletedTask;
        }

        public IQueryable<Track> GetAll(CancellationToken ct)
        {
            return _tracks.AsQueryable();
        }

        public IQueryable<Track> GetByAlbumId(Guid albumId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Track> GetByArtistId(Guid artistId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<Track?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Track> GetByName(string name, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Track> GetByPlaylistId(Guid playlistId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Track entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
