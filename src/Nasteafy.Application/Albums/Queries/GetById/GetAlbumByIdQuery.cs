using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;

namespace Nasteafy.Application.Albums.Queries.GetById
{
    public record GetAlbumByIdQuery(Guid AlbumId) : IRequest<Result<AlbumDto>>;

    public class GetAlbumByIdQueryHandler(IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService)
       : IRequestHandler<GetAlbumByIdQuery, Result<AlbumDto>>
    {
        public async Task<Result<AlbumDto>> Handle(GetAlbumByIdQuery req, CancellationToken ct)
        {
            var album = await unitOfWork.Albums.GetByIdWithIncludeAsync(
                req.AlbumId,
                ct,
                x => x.AlbumArtists,
                x => x.AlbumArtists.Select(at => at.Artist));

            if (album is null)
                return Result.Fail("Album not found")
                    .LogIfFailed<GetAlbumByIdQueryHandler>();

            string? coverUrl = null;
            if (!string.IsNullOrEmpty(album.CoverUrl))
            {
                var result = await fileStorageService.GetFileUrlAsync(FileType.AlbumCover, album.CoverUrl);
                coverUrl = result.Value;
            }

            var albumDto = new AlbumDto(
                album.Id,
                album.Title,
                coverUrl,
                string.Join(", ", album.AlbumArtists.Select(at => at.Artist.Name)));

            return Result.Ok(albumDto);
        }
    }
}


