using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Helpers;
using System.Text.Json.Serialization;

namespace Nasteafy.Application.Playlists.Commands.RemoveFromPlaylist
{
    public class RemoveTrackFromPlaylistCommand : IRequest<Result>, ITransactionalCommand
    {
        [JsonIgnore]
        public Guid PlaylistId { get; set; }
        public Guid TrackId { get; set; }
    }

    public class RemoveTrackFromPlaylistCommandHandler : IRequestHandler<RemoveTrackFromPlaylistCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _userProvider;

        public RemoveTrackFromPlaylistCommandHandler(IUnitOfWork unitOfWork, ICurrentUserProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _userProvider = userProvider;
        }

        public async Task<Result> Handle(RemoveTrackFromPlaylistCommand request, CancellationToken ct)
        {
            var userId = _userProvider.GetUserId();

            if (userId == null || userId == Guid.Empty)
            {
                return Result.Fail("User not authenticated").Log<RemoveTrackFromPlaylistCommandHandler>();
            }                

            var playlist = await _unitOfWork.Playlists.GetByIdWithTracks(request.PlaylistId, ct);

            var track = playlist!.PlaylistTracks.FirstOrDefault(pt => pt.TrackId == request.TrackId);

            playlist.PlaylistTracks.Remove(track!);

            foreach (var playlistTrack in playlist.PlaylistTracks.Where(x => x.Order > track!.Order))
            {
                playlistTrack.Order--;
            }

            await _unitOfWork.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }
}
