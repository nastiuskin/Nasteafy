using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Tracks.Queries.GetByArtistId
{
    public class GetTracksByArtistIdQueryValidator : AbstractValidator<GetTracksByArtistIdQuery>
    {
        public GetTracksByArtistIdQueryValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.ArtistId)
                .NotEmpty()
                    .WithMessage("Artist ID must not be empty.")
                .MustAsync(unitOfWork.Artists.ExistsAsync)
                    .WithMessage("Track not found.");
        }
    }
}

