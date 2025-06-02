using Microsoft.EntityFrameworkCore.Storage;
using Nasteafy.Application.Abstractions.Data.Repositories;

namespace Nasteafy.Application.Abstractions.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IArtistRepository Artists { get; }
        ITrackRepository Tracks { get; }
        IPlaylistRepository Playlists { get; }
        ISubscriptionRepository Subscriptions { get; }

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task SaveChangesAsync(CancellationToken ct);
    }
}
