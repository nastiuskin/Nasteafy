using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Tracks.Commands.Delete
{
    public class DeleteTrackCommandValidator : AbstractValidator<DeleteTrackCommand>
    {
        public DeleteTrackCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.TrackId)
              .NotEmpty()
                .WithMessage("Track ID must not be empty.")
              .MustAsync(unitOfWork.Tracks.ExistsAsync)
                .WithMessage("Track does not exist.");
        }
    }
}
