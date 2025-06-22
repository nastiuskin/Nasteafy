using FluentValidation;

namespace Nasteafy.Application.Artists.Commands.Create
{
    public class CreateArtistCommandValidator : AbstractValidator<CreateArtistCommand>
    {
        public CreateArtistCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");
        }
    }
}
