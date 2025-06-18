using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Tracks.Queries.GetById;

namespace Nasteafy.Application.Tracks.Queries.GetByArtistId;

public record GetTracksByArtistIdQuery(Guid ArtistId, PagedRequest PagedRequest)
    : IRequest<Result<PagedResult<GetTrackDto>>>;

public class GetTracksByArtistIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetTracksByArtistIdQuery, Result<PagedResult<GetTrackDto>>>
{
    public async Task<Result<PagedResult<GetTrackDto>>> Handle(GetTracksByArtistIdQuery request, CancellationToken ct)
    {
        var tracks = await unitOfWork.Tracks
            .GetByArtistIdAsync(request.ArtistId, request.PagedRequest, ct);

        var trackDtos = tracks.Items.Select(track => new GetTrackDto(
            track.Id,
            track.Title,
            string.Join(", ", track.ArtistTracks.Select(at => at.Artist.Name)),
            track.FilePath,
            track.Duration
        )).ToList();

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

