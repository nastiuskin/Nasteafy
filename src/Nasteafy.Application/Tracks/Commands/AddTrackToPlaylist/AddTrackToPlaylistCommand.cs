using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Tracks.Commands.AddTrack
{
    public record AddTrackToPlaylistCommand(Guid PlaylistId, Guid TrackId)
        : IRequest<Result>;

    public class AddTrackToPlaylistCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserProvider userProvider)
            : IRequestHandler<AddTrackToPlaylistCommand, Result>
    {
        public async Task<Result> Handle(AddTrackToPlaylistCommand request, CancellationToken ct)
        {
            var userId = userProvider.GetUserId();
            if (userId == null || userId == Guid.Empty)
                return Result.Fail("User not authenticated")
                    .LogIfFailed<AddTrackToPlaylistCommandHandler>();

            var playlist = await unitOfWork.Playlists.GetByIdWithTracks(request.PlaylistId, ct);
            if (playlist is null)
                return Result.Fail("Playlist not found");

            if (playlist.UserId != userId)
                return Result.Fail("You do not have permission to modify this playlist")
                    .LogIfFailed<AddTrackToPlaylistCommandHandler>();

            var alreadyExists = playlist.PlaylistTracks.Any(pt => pt.TrackId == request.TrackId);
            if (alreadyExists)
                return Result.Fail("Track is already in the playlist")
                     .LogIfFailed<AddTrackToPlaylistCommandHandler>();

            var nextOrder = playlist.PlaylistTracks.Any()
                ? playlist.PlaylistTracks.Max(pt => pt.Order) + 1
                : 1;

            playlist.PlaylistTracks.Add(new PlaylistTrack
            {
                PlaylistId = request.PlaylistId,
                TrackId = request.TrackId,
                Order = nextOrder
            });

            await unitOfWork.SaveChangesAsync(ct);

            return Result.Ok();
        }
    }
}
