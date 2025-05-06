using MediatR;
using Nasteafy.Application.Abstractions;
using Nasteafy.Shared.Dtos.Playlists;

namespace Nasteafy.Application.Playlists.Queries
{
    public record GetPlaylistsByUserIdQuery(Guid UserId) : IRequest<IEnumerable<GetPlaylistDto>>;

    public class GetPlaylistsByUserIdQueryHandler : IRequestHandler<GetPlaylistsByUserIdQuery, IEnumerable<GetPlaylistDto>>
    {
        private readonly IPlaylistRepository _playlistRepository;

        public GetPlaylistsByUserIdQueryHandler(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }

        public async Task<IEnumerable<GetPlaylistDto>> Handle(GetPlaylistsByUserIdQuery request, CancellationToken ct)
        {
            var playlists = await _playlistRepository.GetByUserIdAsync(request.UserId, ct);

            return playlists.Select(x => new GetPlaylistDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ImageUrl = x.CoverUrl
            });
        }
    }
}
