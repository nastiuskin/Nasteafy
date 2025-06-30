using FluentValidation;
using Nasteafy.Application.Playlists.Commands.Delete;
using Nasteafy.Application.Common.Abstractions.Data;

public class DeletePlaylistCommandValidator : AbstractValidator<DeletePlaylistCommand>
{
    public DeletePlaylistCommandValidator(IUnitOfWork unitOfWork)
    {

        RuleFor(x => x.PlaylistId)
            .NotEmpty()
                .WithMessage("Playlist ID must not be empty.")
            .MustAsync(unitOfWork.Playlists.ExistsAsync)
                .WithMessage("Playlist does not exist.");
    }
}
