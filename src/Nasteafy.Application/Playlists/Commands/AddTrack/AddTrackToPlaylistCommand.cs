using FluentResults;
using MediatR;
using Nasteafy.Application.Abstractions.Auth;
using Nasteafy.Application.Abstractions.Data;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Playlists.Commands.AddTrack
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

            //var playlist = _unitOfWork.Playlists.GetByIdWithTracks(request.PlaylistId, ct);
            //if (playlist == null)
            //    return Result.Fail("Playlist not found");

            //if (playlist.UserId != userId)
            //    return Result.Fail("You do not have permission to modify this playlist");

            //var track = await _unitOfWork.Tracks.GetByIdAsync(request.TrackId, ct);
            //if (track == null)
            //    return Result.Fail("Track not found");

            //int nextOrder = playlist.PlaylistTracks.Any()
            // ? playlist.PlaylistTracks.Max(pt => pt.Order) + 1
            // : 1;

            //playlist.PlaylistTracks.Add(new PlaylistTrack
            //{
            //    PlaylistId = playlist.Id,
            //    TrackId = track.Id,
            //    Order = nextOrder
            //});

            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Ok();
        }
    }
}
