using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Playlists.Commands.RemoveFromPlaylist
{
    public class RemoveTrackFromPlaylistCommandValidator : AbstractValidator<RemoveTrackFromPlaylistCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveTrackFromPlaylistCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.PlaylistId)
                .NotEmpty().WithMessage("Playlist ID must not be empty.")
                .MustAsync(PlaylistExistsAsync).WithMessage("Playlist does not exist.");

            RuleFor(x => x.TrackId)
                .NotEmpty().WithMessage("Track ID must not be empty.")
                .MustAsync(TrackExistsAsync).WithMessage("Track does not exist.");
        }

        private async Task<bool> PlaylistExistsAsync(Guid playlistId, CancellationToken ct) =>
            await _unitOfWork.Playlists.ExistsAsync(playlistId, ct);

        private async Task<bool> TrackExistsAsync(Guid trackId, CancellationToken ct) =>
            await _unitOfWork.Tracks.ExistsAsync(trackId, ct);
    }
}
