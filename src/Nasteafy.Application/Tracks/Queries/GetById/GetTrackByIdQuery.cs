using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;

namespace Nasteafy.Application.Tracks.Queries.GetById
{
    public record GetTrackByIdQuery(Guid TrackId) : IRequest<Result<GetTrackDto>>;

    public class GetTrackByIdQueryHandler(IUnitOfWork unitOfWork, 
        IFileStorageService fileStorageService)
    : IRequestHandler<GetTrackByIdQuery, Result<GetTrackDto>>
    {
        public async Task<Result<GetTrackDto>> Handle(GetTrackByIdQuery request, CancellationToken ct)
        {
            var track = await unitOfWork.Tracks.GetByIdWithIncludeAsync(
                request.TrackId,
                ct,
                x => x.ArtistTracks,
                x => x.Album,
                x => x.ArtistTracks.Select(at => at.Artist));

            var fileUrl = !string.IsNullOrEmpty(track?.FilePath)
               ? await fileStorageService.GetFileUrlAsync(FileType.Audio, track?.FilePath)
               : null;

            var albumCoverUrl = !string.IsNullOrEmpty(track?.Album?.CoverUrl)
                ? await fileStorageService.GetFileUrlAsync(FileType.AlbumCover, track?.Album.CoverUrl)
                : null;


            if (track is null)
                return Result.Fail("Track not found")
                    .LogIfFailed<GetTrackByIdQueryHandler>();

            var trackDto = new GetTrackDto(
                 track.Id,
                 track.Title,
                 string.Join(", ", track.ArtistTracks.Select(at => at.Artist.Name)),
                 fileUrl.Value,
                 track.Duration,
                 albumCoverUrl?.Value
             );

            return Result.Ok(trackDto);
        }
    }
}
