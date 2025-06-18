using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Tracks.Queries.GetById;

namespace Nasteafy.Application.Tracks.Queries.GetByAlbumId
{
    public record GetTracksByAlbumIdQuery(Guid AlbumId, PagedRequest PagedRequest)
          : IRequest<Result<PagedResult<GetTrackDto>>>;

    public class GetTracksByAlbumIdQueryHandler(IUnitOfWork unitOfWork)
       : IRequestHandler<GetTracksByAlbumIdQuery, Result<PagedResult<GetTrackDto>>>
    {
        public async Task<Result<PagedResult<GetTrackDto>>> Handle(GetTracksByAlbumIdQuery request, CancellationToken ct)
        {
            var tracks = await unitOfWork.Tracks.GetByAlbumIdAsync(request.AlbumId, request.PagedRequest, ct);

            var trackDtos = tracks.Items.Select(t => new GetTrackDto(
                t.Id,
                t.Title,
                string.Join(", ", t.ArtistTracks.Select(at => at.Artist.Name)),
                t.FilePath,
                t.Duration
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
