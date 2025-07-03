using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Tracks.Commands.AddTrack;

public class AddTrackToPlaylistCommandValidator : AbstractValidator<AddTrackToPlaylistCommand>
{
    public AddTrackToPlaylistCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.PlaylistId)
            .NotEmpty().WithMessage("Playlist ID must not be empty.")
            .MustAsync(unitOfWork.Playlists.ExistsAsync).WithMessage("Playlist does not exist.");

        RuleFor(x => x.TrackId)
            .NotEmpty().WithMessage("Track ID must not be empty.")
            .MustAsync(unitOfWork.Tracks.ExistsAsync).WithMessage("Track does not exist.");
    }
}
