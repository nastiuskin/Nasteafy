using Microsoft.EntityFrameworkCore.Storage;
using Nasteafy.Application.Common.Abstractions.Data.Repositories;

namespace Nasteafy.Application.Common.Abstractions.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IArtistRepository Artists { get; }
        ITrackRepository Tracks { get; }
        IPlaylistRepository Playlists { get; }
        IAlbumRepository Albums { get; }
        ISubscriptionRepository Subscriptions { get; }

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task SaveChangesAsync(CancellationToken ct);
    }
}
