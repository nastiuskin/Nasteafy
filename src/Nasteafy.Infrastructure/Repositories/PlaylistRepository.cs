using Nasteafy.Application.Abstractions;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Persistence.Repositories
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

        public IQueryable<Playlist> GetAll(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<Playlist?> GetById(Guid id, CancellationToken ct)
        {
            return Task.FromResult(_playlists.FirstOrDefault(p => p.Id == id));
        }

        public Task<Playlist?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Playlist> GetByUserId(Guid userId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task RemoveTrackFromPlaylistAsync(Guid playlistId, Guid trackId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Playlist entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Playlist> IBaseRepository<Playlist>.GetAll(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
