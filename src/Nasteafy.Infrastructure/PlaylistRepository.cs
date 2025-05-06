using Nasteafy.Application.Abstractions;
using Nasteafy.Domain.Entities;

namespace Nasteafy.Infrastructure
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly List<Playlist> _playlists = new();

        public Task AddAsync(Playlist entity, CancellationToken ct)
        {
            _playlists.Add(entity);
            return Task.CompletedTask;
        }

        public Task AddTrackToPlaylistAsync(Guid playlistId, Guid trackId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var playlist = _playlists.FirstOrDefault(p => p.Id == id);
            if (playlist is not null)
                _playlists.Remove(playlist);

            return Task.CompletedTask;
        }

        public Task<IQueryable<Playlist>> GetAll(CancellationToken ct)
        {
            return Task.FromResult(_playlists.AsQueryable());
        }

        public Task<Playlist?> GetById(Guid id, CancellationToken ct)
        {
            return Task.FromResult(_playlists.FirstOrDefault(p => p.Id == id));
        }

        public Task<IEnumerable<Playlist>> GetByUserIdAsync(Guid userId, CancellationToken ct)
        {
            var result = _playlists.Where(p => p.UserId == userId);
            return Task.FromResult(result.AsEnumerable());
        }

        public Task<IEnumerable<Playlist>> GetPublicPlaylistsAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task RemoveTrackFromPlaylistAsync(Guid playlistId, Guid trackId, CancellationToken ct)
        {
            var playlist = _playlists.FirstOrDefault(p => p.Id == playlistId);
            if (playlist is not null)
            {
                var track = playlist.Tracks.FirstOrDefault(t => t.Id == trackId);
                if (track is not null)
                    playlist.Tracks.Remove(track);
            }

            return Task.CompletedTask;
        }

        public Task UpdateAsync(Playlist entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
