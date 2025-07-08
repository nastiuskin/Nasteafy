using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Helpers;
using Nasteafy.Domain.Entities.Tracks;
using System.Text.Json.Serialization;

namespace Nasteafy.Application.Playlists.Commands.AddTrackToPlaylist
{
    public class AddTrackToPlaylistCommand : IRequest<Result>, ITransactionalCommand
    {
        [JsonIgnore]
        public Guid PlaylistId { get; set; }
        public Guid TrackId { get; set; }
    }
       

    public class AddTrackToPlaylistCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserProvider userProvider)
            : IRequestHandler<AddTrackToPlaylistCommand, Result>
    {
        public async Task<Result> Handle(AddTrackToPlaylistCommand request, CancellationToken ct)
        {
            var userId = userProvider.GetUserId();

            if (userId == null || userId == Guid.Empty)
            {
                return Result.Fail("User not authenticated").Log<AddTrackToPlaylistCommandHandler>();
            }

            var playlist = await unitOfWork.Playlists.GetByIdWithTracks(request.PlaylistId, ct);

            if (playlist!.UserId != userId)
            {
                return Result.Fail("You do not have permission to modify this playlist").Log<AddTrackToPlaylistCommandHandler>();
            }                

            var alreadyExists = playlist.PlaylistTracks.Any(pt => pt.TrackId == request.TrackId);

            if (alreadyExists)
            {
                return Result.Fail("Track is already in the playlist").Log<AddTrackToPlaylistCommandHandler>();
            }                

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
