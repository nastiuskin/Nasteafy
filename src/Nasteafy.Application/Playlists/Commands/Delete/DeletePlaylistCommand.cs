using FluentResults;
using MediatR;
using Nasteafy.Application.Abstractions.Auth;
using Nasteafy.Application.Abstractions.Data;

namespace Nasteafy.Application.Playlists.Commands.Delete
{
    public record DeletePlaylistCommand(Guid PlaylistId) : IRequest<Result>;

    public class DeletePlaylistCommandHandler : IRequestHandler<DeletePlaylistCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserIdProvider _userProvider;

        public DeletePlaylistCommandHandler(IUnitOfWork unitOfWork, IUserIdProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _userProvider = userProvider;
        }

        public async Task<Result> Handle(DeletePlaylistCommand request, CancellationToken ct)
        {
            await _unitOfWork.Playlists.DeleteAsync(request.PlaylistId, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Ok();
        }
    }
}
