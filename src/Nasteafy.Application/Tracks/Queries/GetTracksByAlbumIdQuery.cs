using MediatR;
using Nasteafy.Application.Abstractions;
using Nasteafy.Shared.Dtos.Tracks;

namespace Nasteafy.Application.Tracks.Queries
{
    public record GetTracksByAlbumIdQuery(Guid AlbumId) : IRequest<IEnumerable<GetTrackDto>>;

    public class GetTracksByAlbumIdQueryHandler : IRequestHandler<GetTracksByAlbumIdQuery, IEnumerable<GetTrackDto>>
    {
        private readonly ITrackRepository _trackRepository;
        public GetTracksByAlbumIdQueryHandler(ITrackRepository trackRepository)
        {
            _trackRepository = trackRepository;
        }

        public async Task<IEnumerable<GetTrackDto>> Handle(GetTracksByAlbumIdQuery request, CancellationToken ct)
        {
            var tracks = await _trackRepository.GetByAlbumIdAsync(request.AlbumId, ct);

            return tracks.Select(track => new GetTrackDto
            {
                Id = track.Id,
                Title = track.Title,
                Duration = track.Duration,
                ArtistName = track.Artist.Name
            });
        }
    }
}
