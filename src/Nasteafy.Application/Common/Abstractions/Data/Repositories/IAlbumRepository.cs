using Nasteafy.Application.Common.Models;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Common.Abstractions.Data.Repositories
{
    public interface IAlbumRepository : IGenericRepository<Album>
    {
        Task<PagedResult<Album>> GetAlbumsByArtistIdAsync(Guid artistId, PagedRequest request, CancellationToken ct);
        Task<PagedResult<Album>> GetAllAlbumsWithRatings(PagedRequest request, CancellationToken ct);
    }
}

