using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;

namespace Nasteafy.Application.Tracks.Queries.GetById
{
    public record GetTrackByIdQuery(Guid TrackId) : IRequest<Result<GetTrackDto>>;

    public class GetTrackByIdQueryHandler(IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ICurrentUserProvider userProvider) : IRequestHandler<GetTrackByIdQuery, Result<GetTrackDto>>
    {
        public async Task<Result<GetTrackDto>> Handle(GetTrackByIdQuery request, CancellationToken ct)
        {
            var track = await unitOfWork.Tracks.GetByIdWithIncludeAsync(
                 request.TrackId,
                 ct,
                 q => q.Include(x => x.ArtistTracks)
                         .ThenInclude(at => at.Artist)
                     .Include(x => x.Album));

            var userId = userProvider.GetUserId();

            var getFileResult = await fileStorageService.GetFileUrlAsync(FileType.Audio, track!.FilePath);
            if (getFileResult.IsFailed)
            {
                return Result.Fail(getFileResult.Errors.ToList());
            }

            var albumCoverUrl = !string.IsNullOrEmpty(track?.Album?.CoverUrl)
                ? await fileStorageService.GetFileUrlAsync(FileType.AlbumCover, track?.Album.CoverUrl)
                : null;          

            var trackDto = new GetTrackDto(
                 track!.Id,
                 track.Title,
                 string.Join(", ", track.ArtistTracks.Select(at => at.Artist.Name)),
                 getFileResult.Value,
                 track.Duration,
                 albumCoverUrl?.Value,
                 track.TrackLikes.Any(l => l.UserId == userId)
             );

            return Result.Ok(trackDto);
        }
    }
}
