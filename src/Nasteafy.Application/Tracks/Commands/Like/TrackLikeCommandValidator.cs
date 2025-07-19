using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Tracks.Commands.Like
{
    public class TrackLikeCommandValidator : AbstractValidator<TrackLikeCommand>
    {
        public TrackLikeCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.TrackId)
                 .NotEmpty().WithMessage("TrackId is required")
                 .MustAsync(unitOfWork.Tracks.ExistsAsync).WithMessage("Track does not exist");
        }
    }
}
