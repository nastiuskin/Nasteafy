using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Tracks.Commands.AddTrackToAlbum
{
    public class AddTrackToAlbumCommandValidator : AbstractValidator<AddTrackToAlbumCommand>
    {
        public AddTrackToAlbumCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.AlbumId)
                .NotEmpty()
                    .WithMessage("Album ID must not be empty.")
                .MustAsync(unitOfWork.Albums.ExistsAsync)
                    .WithMessage("Album does not exist.");

            RuleFor(x => x.ArtistIds)
              .NotEmpty()
                    .WithMessage("At least one artist must be assigned.");

            RuleFor(x => x.Track)
                .NotNull()
                    .WithMessage("Track is required");
        }
    }
}
