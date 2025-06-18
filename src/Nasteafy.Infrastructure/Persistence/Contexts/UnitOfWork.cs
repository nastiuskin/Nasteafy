using Microsoft.EntityFrameworkCore.Storage;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Data.Repositories;

namespace Nasteafy.Infrastructure.Persistence.Contexts
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;

        public IUserRepository Users { get; }
        public IArtistRepository Artists { get; }
        public ITrackRepository Tracks { get; }
        public IAlbumRepository Albums { get; }
        public IPlaylistRepository Playlists { get; }
        public ISubscriptionRepository Subscriptions { get; }

        public UnitOfWork(DatabaseContext context,
            ITrackRepository tracks,
            IPlaylistRepository playlists,
            IArtistRepository artists,
            IAlbumRepository albums,
            IUserRepository users,
            ISubscriptionRepository subscriptions)
        {
            _context = context;
            Tracks = tracks;
            Playlists = playlists;
            Artists = artists;
            Albums = albums;
            Users = users;
            Subscriptions = subscriptions;
        }

        public async Task SaveChangesAsync(CancellationToken ct)
        {
            await _context.SaveChangesAsync(ct);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}