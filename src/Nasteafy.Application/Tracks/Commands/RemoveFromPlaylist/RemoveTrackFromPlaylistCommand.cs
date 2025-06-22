using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Tracks.Commands.RemoveFromPlaylist
{
    public record RemoveTrackFromPlaylistCommand(Guid PlaylistId, Guid TrackId) : IRequest<Result>;

    public class RemoveTrackFromPlaylistCommandHandler : IRequestHandler<RemoveTrackFromPlaylistCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserIdProvider _userProvider;

        public RemoveTrackFromPlaylistCommandHandler(IUnitOfWork unitOfWork, IUserIdProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _userProvider = userProvider;
        }

        public async Task<Result> Handle(RemoveTrackFromPlaylistCommand request, CancellationToken ct)
        {
            var userId = _userProvider.GetUserId();
            if (userId == null || userId == Guid.Empty)
                return Result.Fail("User not authenticated")
                    .LogIfFailed<RemoveTrackFromPlaylistCommandHandler>();

            var playlist = await _unitOfWork.Playlists.GetByIdWithTracks(request.PlaylistId, ct);
            if (playlist is null)
                return Result.Fail("Playlist not found");

            if (playlist.UserId != userId)
                return Result.Fail("You do not have permission to modify this playlist")
                    .LogIfFailed<RemoveTrackFromPlaylistCommandHandler>();

            var track = playlist.PlaylistTracks.FirstOrDefault(pt => pt.TrackId == request.TrackId);
            if (track is null)
                return Result.Fail("Track is not in the playlist")
                    .LogIfFailed<RemoveTrackFromPlaylistCommandHandler>();

            playlist.PlaylistTracks.Remove(track);

            foreach (var playlistTrack in playlist.PlaylistTracks.Where(x => x.Order > track.Order))
            {
                playlistTrack.Order--;
            }

            await _unitOfWork.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }
}
