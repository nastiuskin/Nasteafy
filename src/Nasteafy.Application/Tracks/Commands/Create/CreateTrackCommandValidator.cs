using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Tracks.Commands.Create
{
    public class CreateTrackCommandValidator : AbstractValidator<CreateTrackCommand>
    {
        public CreateTrackCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("Audio file is required")
                .Must(f => f.Length > 0).WithMessage("Audio is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200);

            RuleFor(x => x.Duration)
                .GreaterThan(TimeSpan.Zero).WithMessage("Duration must be positive.");

            RuleFor(x => x.Artists)
                .NotEmpty().WithMessage("At least one artist must be assigned.");
        }
    }
}
