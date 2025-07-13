using FluentValidation;

namespace Nasteafy.Application.Playlists.Commands.Create
{
    public class CreatePlaylistCommandValidator : AbstractValidator<CreatePlaylistCommand>
    {
        public CreatePlaylistCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required");   
        }
    }
}
