using FluentResults;
using MediatR;
using Nasteafy.Application.Albums.Queries.GetById;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Models;
using Nasteafy.Domain;

namespace Nasteafy.Application.Albums.Queries.GetByArtistId
{
    public record GetByArtistIdQuery(Guid ArtistId, PagedRequest PagedRequest) : IRequest<Result<PagedResult<AlbumDto>>>;

    public class GetByArtistIdQueryHandler(
   IUnitOfWork unitOfWork,
   IFileStorageService fileStorageService)
        : IRequestHandler<GetByArtistIdQuery, Result<PagedResult<AlbumDto>>>
    {
        public async Task<Result<PagedResult<AlbumDto>>> Handle(GetByArtistIdQuery query, CancellationToken ct)
        {
            var albums = await unitOfWork.Albums.GetByArtistIdAsync(query.ArtistId, query.PagedRequest, ct);

            var albumDtos = new List<AlbumDto>();

            foreach (var a in albums.Items)
            {
                var coverUrl = !string.IsNullOrEmpty(a.CoverUrl)
                    ? await fileStorageService.GetFileUrlAsync(FileType.AlbumCover, a.CoverUrl)
                    : null;

                var artistNames = a.AlbumArtists != null
                ? string.Join(", ", a.AlbumArtists
                    .Select(aa => aa.Artist?.Name)
                    .Where(name => !string.IsNullOrEmpty(name)))
                : string.Empty;

                albumDtos.Add(new AlbumDto(
                    a.Id,
                    a.Title,
                    coverUrl?.Value,
                    artistNames));
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
