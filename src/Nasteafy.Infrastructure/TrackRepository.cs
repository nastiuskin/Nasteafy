using Nasteafy.Application.Abstractions;
using Nasteafy.Domain.Entities;

namespace Nasteafy.Infrastructure
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

        public Task<IQueryable<Track>> GetAll(CancellationToken ct)
        {
            return Task.FromResult(_tracks.AsQueryable());
        }

        public Task<IEnumerable<Track>> GetByAlbumIdAsync(Guid albumId, CancellationToken ct)
        {
            var albumTracks = _tracks.Where(x => x.AlbumId == albumId); 
            return Task.FromResult(albumTracks);
        }

        public Task<IEnumerable<Track>> GetByArtistIdAsync(Guid artistId, CancellationToken ct)
        {
            var artistTracks = _tracks.Where(x =>x.ArtistId == artistId);   
            return Task.FromResult(artistTracks);
        }

        public Task<IEnumerable<Track>> GetByGenreAsync(Guid genreId, CancellationToken ct)
        {
            var result = _tracks.Where(t => t.Genres.Any(g => g.Id == genreId));
            return Task.FromResult(result);
        }

        public Task<Track?> GetById(Guid id, CancellationToken ct)
        {
            var track = _tracks.FirstOrDefault(t => t.Id == id);
            return Task.FromResult(track);
        }

        public Task<IEnumerable<Track>> GetByNameAsync(string title, CancellationToken ct)
        {
            var result = _tracks.Where(t => t.Title.Contains(title));
            return Task.FromResult(result);
        }

        public Task<IEnumerable<Track>> GetByPlaylistIdAsync(Guid playlistId, CancellationToken ct)
        {
            var result = _tracks.Where(t => t.Playlists.Any(p => p.Id == playlistId));
            return Task.FromResult(result);
        }

        public Task UpdateAsync(Track entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
