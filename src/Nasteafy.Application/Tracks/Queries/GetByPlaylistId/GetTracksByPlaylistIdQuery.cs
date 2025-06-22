using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Playlists.Queries.GetByUserId;
using Nasteafy.Application.Tracks.Queries.GetById;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Domain;

namespace Nasteafy.Application.Tracks.Queries.GetByPlaylistId
{
    public record GetTracksByPlaylistIdQuery(Guid PlaylistId, PagedRequest PagedRequest)
     : IRequest<Result<PagedResult<GetTrackDto>>>;

    public class GetTracksByArtistIdQueryHandler(IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService)
        : IRequestHandler<GetTracksByPlaylistIdQuery, Result<PagedResult<GetTrackDto>>>
    {
        public async Task<Result<PagedResult<GetTrackDto>>> Handle(GetTracksByPlaylistIdQuery request, CancellationToken ct)
        {
            var tracks = await unitOfWork.Tracks
                .GetByPlaylistIdAsync(request.PlaylistId, request.PagedRequest, ct);

            var trackDtos = new List<GetTrackDto>();

            foreach (var t in tracks.Items)
            {
                var fileUrl = !string.IsNullOrEmpty(t.FilePath)
                    ? await fileStorageService.GetFileUrlAsync(FileType.Audio, t.FilePath)
                    : null;

                var albumCoverUrl = !string.IsNullOrEmpty(t.Album?.CoverUrl)
                    ? await fileStorageService.GetFileUrlAsync(FileType.AlbumCover, t.Album.CoverUrl)
                    : null;

                trackDtos.Add(new GetTrackDto(
                    t.Id,
                    t.Title,
                    string.Join(", ", t.ArtistTracks
                        .Where(at => at.Artist != null)
                        .Select(at => at.Artist.Name)),
                    fileUrl!.Value,
                    t.Duration,
                    albumCoverUrl?.Value));
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

