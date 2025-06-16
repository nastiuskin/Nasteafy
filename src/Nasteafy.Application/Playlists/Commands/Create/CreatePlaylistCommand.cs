using FluentResults;
using MediatR;
using Nasteafy.Application.Abstractions.Auth;
using Nasteafy.Application.Abstractions.Data;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Playlists.Commands.Create
{
    public record CreatePlaylistCommand(string Title) : IRequest<Result<Guid>>;

    public class CreatePlaylistCommandHandler : IRequestHandler<CreatePlaylistCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserIdProvider _userProvider;

        public CreatePlaylistCommandHandler(IUnitOfWork unitOfWork, IUserIdProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _userProvider = userProvider;
        }

        public async Task<Result<Guid>> Handle(CreatePlaylistCommand request, CancellationToken cancellationToken)
        {
            var userId = _userProvider.GetUserId();

            if (userId == null || userId == Guid.Empty)
                return Result.Fail("UserId not found");

            var playlist = new Playlist
            {
                Title = request.Title,
                UserId = userId ?? Guid.Empty,
                PlaylistTracks = new List<PlaylistTrack>()
            };

            await _unitOfWork.Playlists.AddAsync(playlist, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Ok(playlist.Id);
        }
    }
}
