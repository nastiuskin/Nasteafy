using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Albums.Commands.Create
{
    public class CreateAlbumCommand : IRequest<Result<Guid>>
    {
        public required string Title { get; init; }
        public IFormFile? CoverFile { get; init; }
        public required DateTime  ReleaseDate { get; init; }
        public List<Guid> Artists { get; init; } = [];
    }

    public class CreateAlbumCommandHandler(IUnitOfWork unitOfWork,
         IFileStorageService fileStorageService)
        : IRequestHandler<CreateAlbumCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateAlbumCommand request, CancellationToken ct)
        {
            var artistIds = await unitOfWork.Artists
              .FindAllByIds(request.Artists)
              .Select(x => x.Id)
              .ToListAsync(ct);

            if (artistIds.Count != request.Artists.Count)
                return Result.Fail("Some of the specified artists were not found.").Log<CreateAlbumCommandHandler>();

            string? coverUrl = null;

            if (request.CoverFile is not null && request.CoverFile.Length > 0)
            {
                await using var stream = request.CoverFile.OpenReadStream();
                var uploadResult = await fileStorageService.UploadFileAsync(
                    stream,
                    request.CoverFile.FileName,
                    request.CoverFile.ContentType,
                    FileType.AlbumCover);

                if (uploadResult.IsSuccess)
                    coverUrl = uploadResult.Value;
            }

            var albumId = Guid.NewGuid();
            var album = new Album
            {
                Id = albumId,
                Title = request.Title,
                ReleaseDate = request.ReleaseDate.ToUniversalTime(),
                CoverUrl = coverUrl,
                AlbumArtists = artistIds.Select(artistId => new AlbumArtist
                {
                    AlbumId = albumId,
                    ArtistId = artistId
                }).ToList(),
            };

            await unitOfWork.Albums.AddAsync(album, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Ok(album.Id);
        }
    }
}
