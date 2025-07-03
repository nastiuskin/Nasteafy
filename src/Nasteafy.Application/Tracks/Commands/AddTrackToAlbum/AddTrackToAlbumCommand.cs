using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;
using Nasteafy.Domain.Entities;
using Nasteafy.Domain.Entities.Tracks;
using System.Text.Json.Serialization;

namespace Nasteafy.Application.Tracks.Commands.AddTrackToAlbum
{
    public class AddTrackToAlbumCommand : IRequest<Result<Guid>>
    {
        [JsonIgnore]
        public Guid AlbumId { get; set; }
        public IFormFile Track { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public List<Guid> ArtistIds { get; set; }
    }


    public class AddTrackToAlbumCommandHandler(IUnitOfWork unitOfWork, IFileStorageService fileStorageService)
        : IRequestHandler<AddTrackToAlbumCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(AddTrackToAlbumCommand request, CancellationToken ct)
        {
            var album = await unitOfWork.Albums.GetByIdAsync(request.AlbumId, ct);

            var artistIds = await unitOfWork.Artists
               .FindAllByIds(request.ArtistIds)
               .Select(x => x.Id)
               .ToListAsync(ct);

            if (artistIds.Count != request.ArtistIds.Count)
                return Result.Fail("Some of the specified artists were not found.").Log<AddTrackToAlbumCommand>();

            await using var stream = request.Track.OpenReadStream();
            var uploadResult = await fileStorageService.UploadFileAsync(
                stream,
                request.Track.FileName,
                request.Track.ContentType,
                FileType.Audio);

            if (!uploadResult.IsSuccess)
                return Result.Fail("Failed to upload file").Log<AddTrackToAlbumCommand>();

            var trackId = Guid.NewGuid();

            var track = new Track
            {
                Id = trackId,
                Title = request.Title,
                Duration = request.Duration,
                FilePath = uploadResult.Value,
                AlbumId = request.AlbumId,
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

