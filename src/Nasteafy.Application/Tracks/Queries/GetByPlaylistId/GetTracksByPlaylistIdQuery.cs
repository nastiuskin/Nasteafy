using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Tracks.Queries.GetById;

namespace Nasteafy.Application.Tracks.Queries.GetByPlaylistId
{
    public record GetTracksByPlaylistIdQuery(Guid PlaylistId, PagedRequest PagedRequest)
     : IRequest<Result<PagedResult<GetTrackDto>>>;

    public class GetTracksByArtistIdQueryHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetTracksByPlaylistIdQuery, Result<PagedResult<GetTrackDto>>>
    {
        public async Task<Result<PagedResult<GetTrackDto>>> Handle(GetTracksByPlaylistIdQuery request, CancellationToken ct)
        {
            var tracks = await unitOfWork.Tracks
                .GetByPlaylistIdAsync(request.PlaylistId, request.PagedRequest, ct);

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
}

