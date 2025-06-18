using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Tracks.Commands.AddTrack
{
    public record AddTrackToPlaylistCommand(Guid PlaylistId, Guid TrackId) : IRequest<Result>;

    public class AddTrackToPlaylistCommandHandler : IRequestHandler<AddTrackToPlaylistCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserIdProvider _userProvider;

        public AddTrackToPlaylistCommandHandler(IUnitOfWork unitOfWork, IUserIdProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _userProvider = userProvider;
        }

        public async Task<Result> Handle(AddTrackToPlaylistCommand request, CancellationToken ct)
        {
            var userId = _userProvider.GetUserId();
            if (userId == null || userId == Guid.Empty)
                return Result.Fail("User not authenticated");

            var playlist = await _unitOfWork.Playlists.GetByIdWithTracks(request.PlaylistId, ct);
            if (playlist is null)
                return Result.Fail("Playlist not found");

            if (playlist.UserId != userId)
                return Result.Fail("You do not have permission to modify this playlist");

            var alreadyExists = playlist.PlaylistTracks.Any(pt => pt.TrackId == request.TrackId);
            if (alreadyExists)
                return Result.Fail("Track is already in the playlist");

            var nextOrder = playlist.PlaylistTracks.Any()
                ? playlist.PlaylistTracks.Max(pt => pt.Order) + 1
                : 1;

            playlist.PlaylistTracks.Add(new PlaylistTrack
            {
                PlaylistId = request.PlaylistId,
                TrackId = request.TrackId,
                Order = nextOrder
            });

            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Ok();
        }
    }
}
