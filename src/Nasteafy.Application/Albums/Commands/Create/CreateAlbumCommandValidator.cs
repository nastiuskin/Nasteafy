using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Albums.Commands.Create
{
    public class CreateAlbumCommandValidator : AbstractValidator<CreateAlbumCommand>
    {
        public CreateAlbumCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.Title)
             .NotEmpty()
             .WithMessage("Album title is required.");

            RuleFor(x => x.ReleaseDate)
             .NotEmpty()
             .WithMessage("Release date is required.")
             .LessThanOrEqualTo(DateTime.UtcNow)
             .WithMessage("Release date cannot be in the future.");

            RuleFor(x => x.Artists)
            .NotEmpty()
            .WithMessage("At least one artist must be assigned.");
        }
    }
}
