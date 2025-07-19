using FluentResults;
using MediatR;
using Nasteafy.Application.Albums.Queries.GetById;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Models;
using Nasteafy.Domain;

namespace Nasteafy.Application.Albums.Queries.GetAll
{
    public record GetAllAlbumsQuery(PagedRequest PagedRequest)
          : IRequest<Result<PagedResult<AlbumDto>>>;

    public class GetAllAlbumsQueryHandler(IUnitOfWork unitOfWork,
       IFileStorageService fileStorageService,
       ICurrentUserProvider userProvider)
      : IRequestHandler<GetAllAlbumsQuery, Result<PagedResult<AlbumDto>>>
    {
        public async Task<Result<PagedResult<AlbumDto>>> Handle(GetAllAlbumsQuery request, CancellationToken ct)
        {
            var albums = await unitOfWork.Albums.GetAllAlbumsWithRatings(request.PagedRequest, ct);

            var albumDtos = new List<AlbumDto>();

            var userId = userProvider.GetUserId();

            foreach (var a in albums.Items)
            {
                var albumCoverUrl = !string.IsNullOrEmpty(a.CoverUrl)
                    ? await fileStorageService.GetFileUrlAsync(FileType.AlbumCover, a.CoverUrl)
                    : null;

                var ratings = a.Ratings ?? [];

                var averageRating = ratings.Count > 0 ? Math.Round(ratings.Average(r => r.Rating), 1) : 0;
                var userRating = userId != Guid.Empty ? ratings.FirstOrDefault(r => r.UserId == userId)?.Rating : null;

                var artists = string.Join(", ", a.AlbumArtists
                        .Where(at => at.Artist != null)
                        .Select(at => at.Artist.Name));

                albumDtos.Add(new AlbumDto(a.Id, a.Title, a.ReleaseDate, albumCoverUrl?.Value, artists, averageRating, userRating));
            }

            var result = new PagedResult<AlbumDto>
            {
                Items = albumDtos,
                TotalItems = albums.TotalItems,
                PageNumber = albums.PageNumber,
                PageSize = albums.PageSize
            };

            return Result.Ok(result);
        }
    }
}
