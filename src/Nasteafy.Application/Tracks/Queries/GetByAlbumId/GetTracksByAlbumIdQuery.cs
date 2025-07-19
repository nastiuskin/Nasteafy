using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Tracks.Queries.GetById;
using Nasteafy.Domain;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Application.Tracks.Queries.GetByAlbumId
{
    public record GetTracksByAlbumIdQuery(Guid AlbumId, PagedRequest PagedRequest)
          : IRequest<Result<PagedResult<GetTrackDto>>>; 

    public class GetTracksByAlbumIdQueryHandler(IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ICurrentUserProvider userProvider)
       : IRequestHandler<GetTracksByAlbumIdQuery, Result<PagedResult<GetTrackDto>>>
    {
        public async Task<Result<PagedResult<GetTrackDto>>> Handle(GetTracksByAlbumIdQuery request, CancellationToken ct)
        {
            var tracks = await unitOfWork.Tracks.GetByAlbumIdAsync(request.AlbumId, request.PagedRequest, ct);
            var userId = userProvider.GetUserId();

            var trackDtos = new List<GetTrackDto>();

            foreach (var t in tracks.Items)
            {
                var getFileResult = await fileStorageService.GetFileUrlAsync(FileType.Audio, t.FilePath);
                if(getFileResult.IsFailed)
                {
                    continue;
                }

                var albumCoverUrl = !string.IsNullOrEmpty(t.Album?.CoverUrl)
                    ? await fileStorageService.GetFileUrlAsync(FileType.AlbumCover, t.Album.CoverUrl)
                    : null;

                trackDtos.Add(new GetTrackDto(
                    t.Id,
                    t.Title,
                    string.Join(", ", t.ArtistTracks
                        .Where(at => at.Artist != null)
                        .Select(at => at.Artist.Name)),
                    getFileResult!.Value,
                    t.Duration,
                    albumCoverUrl?.Value,
                    t.TrackLikes.Any(l => l.UserId == userId)));
            }

            var result = new PagedResult<GetTrackDto>
            {
                Items = trackDtos,
                TotalItems = tracks.TotalItems,
                PageNumber = tracks.PageNumber,
                PageSize = tracks.PageSize
            };

            return Result.Ok(result);
        }
    }
}
