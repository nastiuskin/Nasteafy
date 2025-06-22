using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Tracks.Queries.GetById;
using Nasteafy.Domain;

namespace Nasteafy.Application.Tracks.Queries.GetByArtistId;

public record GetTracksByArtistIdQuery(Guid ArtistId, PagedRequest PagedRequest)
    : IRequest<Result<PagedResult<GetTrackDto>>>;

public class GetTracksByArtistIdQueryHandler(IUnitOfWork unitOfWork,
    IFileStorageService fileStorageService)
    : IRequestHandler<GetTracksByArtistIdQuery, Result<PagedResult<GetTrackDto>>>
{
    public async Task<Result<PagedResult<GetTrackDto>>> Handle(GetTracksByArtistIdQuery request, CancellationToken ct)
    {
        var tracks = await unitOfWork.Tracks
            .GetByArtistIdAsync(request.ArtistId, request.PagedRequest, ct);

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

