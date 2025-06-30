using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Playlists.Commands.Create
{
    public record CreatePlaylistCommand(string Title) : IRequest<Result<Guid>>;

    public class CreatePlaylistCommandHandler : IRequestHandler<CreatePlaylistCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _userProvider;

        public CreatePlaylistCommandHandler(IUnitOfWork unitOfWork, ICurrentUserProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _userProvider = userProvider;
        }

        public async Task<Result<Guid>> Handle(CreatePlaylistCommand request, CancellationToken cancellationToken)
        {
            var userId = _userProvider.GetUserId();

            if (userId == null || userId == Guid.Empty)
                return Result.Fail("UserId not found").Log<CreatePlaylistCommandHandler>();

            var playlist = new Playlist
            {
                Title = request.Title,
                UserId = userId.Value,
            };

            await _unitOfWork.Playlists.AddAsync(playlist, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Ok(playlist.Id);
        }
    }
}
