using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Tracks.Commands.Create
{
    public class CreateTrackCommandValidator : AbstractValidator<CreateTrackCommand>
    {
        public CreateTrackCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.File)
                .NotNull()
                    .WithMessage("Audio file is required")
                .Must(f => f.Length > 0)
                    .WithMessage("Audio file cannot be empty.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(200);

            RuleFor(x => x.Duration)
                .GreaterThan(TimeSpan.Zero)
                .WithMessage("Duration must be positive.");

            RuleFor(x => x.ArtistIds)
                .NotEmpty()
                .WithMessage("At least one artist must be assigned.");
        }
    }
}
