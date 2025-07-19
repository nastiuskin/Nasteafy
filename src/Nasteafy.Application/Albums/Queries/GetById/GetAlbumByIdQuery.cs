using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Albums.Queries.GetById
{
    public record GetAlbumByIdQuery(Guid AlbumId) : IRequest<Result<AlbumDto>>;

    public class GetAlbumByIdQueryHandler(IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ICurrentUserProvider userProvider) : IRequestHandler<GetAlbumByIdQuery, Result<AlbumDto>>
    {
        public async Task<Result<AlbumDto>> Handle(GetAlbumByIdQuery req, CancellationToken ct)
        {
            var album = await unitOfWork.Albums.GetByIdWithIncludeAsync(
              req.AlbumId,
              ct,
              q => q
                  .Include(x => x.AlbumArtists)
                      .ThenInclude(at => at.Artist)
                  .Include(x => x.Ratings));

            var userId = userProvider.GetUserId();

            string? coverUrl = null;
            if (!string.IsNullOrEmpty(album!.CoverUrl))
            {
                var result = await fileStorageService.GetFileUrlAsync(FileType.AlbumCover, album.CoverUrl);
                coverUrl = result.Value;
            }

            var artists = album.AlbumArtists != null
                    ? string.Join(", ", album.AlbumArtists
                        .Select(aa => aa.Artist?.Name)
                        .Where(name => !string.IsNullOrEmpty(name)))
                    : string.Empty;

            var averageRating = album.Ratings.Any() ? Math.Round(album.Ratings.Average(x => x.Rating), 1) : 0;
            var userRating = userId == Guid.Empty
                ? null
                : album.Ratings.FirstOrDefault(x => x.UserId == userId)?.Rating;

            var albumDto = new AlbumDto(album.Id, album.Title, album.ReleaseDate, coverUrl, artists, averageRating, userRating);
            return Result.Ok(albumDto);
        }
    }
}


