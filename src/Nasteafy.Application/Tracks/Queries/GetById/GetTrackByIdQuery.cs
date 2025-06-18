using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Tracks.Queries.GetById
{
    public record GetTrackByIdQuery(Guid TrackId) : IRequest<Result<GetTrackDto>>;

    public class GetTrackByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetTrackByIdQuery, Result<GetTrackDto>>
    {
        public async Task<Result<GetTrackDto>> Handle(GetTrackByIdQuery request, CancellationToken ct)
        {
            var track = await unitOfWork.Tracks.GetByIdWithIncludeAsync(
                request.TrackId,
                ct,
                x => x.ArtistTracks,
                x => x.ArtistTracks.Select(at => at.Artist));

            if (track is null)
                return Result.Fail("Track not found");

            var trackDto = new GetTrackDto(
                 track.Id,
                 track.Title,
                 string.Join(", ", track.ArtistTracks.Select(at => at.Artist.Name)),
                 track.FilePath,
                 track.Duration
             );

            return Result.Ok(trackDto);
        }
    }
}
