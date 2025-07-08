using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Helpers;
using Nasteafy.Domain;
using Nasteafy.Domain.Entities;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Tracks.Commands.Create
{
    public record CreateTrackCommand(
          IFormFile File,
          string Title,
          TimeSpan Duration,
          Guid? AlbumId,
          List<Guid> ArtistIds) : IRequest<Result<Guid>>, ITransactionalCommand;

    public class CreateTrackCommandHandler(IUnitOfWork unitOfWork, IFileStorageService _fileStorage)
      : IRequestHandler<CreateTrackCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateTrackCommand request, CancellationToken ct)
        {
            var artistIds = await unitOfWork.Artists
                .FindAllByIds(request.ArtistIds)
                .Select(x => x.Id)
                .ToListAsync(ct);

            if (artistIds.Count != request.ArtistIds.Count)
            {
                return Result.Fail("Some of the specified artists were not found.").Log<CreateTrackCommandHandler>();
            }               

            await using var stream = request.File.OpenReadStream();
            var uploadResult = await _fileStorage.UploadFileAsync(
                stream,
                request.File.FileName,
                request.File.ContentType,
                FileType.Audio);

            if (!uploadResult.IsSuccess)
            {
                return Result.Fail("Failed to upload file").Log<CreateTrackCommandHandler>();
            }                

            var trackId = Guid.NewGuid();

            var track = new Track
            {
                Id = trackId,
                Title = request.Title,
                Duration = request.Duration,
                FilePath = uploadResult.Value,
                AlbumId = request.AlbumId ?? null,
                ArtistTracks = artistIds
                    .Select(artist => new ArtistTrack
                    {
                        TrackId = trackId,
                        ArtistId = artist
                    })
                    .ToList()
            };

            await unitOfWork.Tracks.AddAsync(track, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Ok(track.Id);
        }
    }
}
