using Microsoft.EntityFrameworkCore.Storage;
using Nasteafy.Application.Abstractions;
using Nasteafy.Application.Abstractions.Data;
using Nasteafy.Application.Abstractions.Data.Repositories;
using Nasteafy.Infrastructure.Database;

namespace Nasteafy.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;

        public IUserRepository Users { get; }
        public IArtistRepository Artists { get; }
        public ITrackRepository Tracks { get; }
        public IPlaylistRepository Playlists { get; }
        public ISubscriptionRepository Subscriptions { get; }

        public UnitOfWork(DatabaseContext context,
            ITrackRepository tracks,
            IPlaylistRepository playlists,
            IArtistRepository artists,
            IUserRepository users)
        {
            _context = context;
            Tracks = tracks;
            Playlists = playlists;
            Artists = artists;
            Users = users;
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