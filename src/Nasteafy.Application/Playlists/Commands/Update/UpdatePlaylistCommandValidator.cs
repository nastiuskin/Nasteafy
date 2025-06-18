using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Playlists.Commands.Update
{
    public class UpdatPlaylistCommandValidator : AbstractValidator<UpdatePlaylistCommand>
    {
        public UpdatPlaylistCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.PlaylistId)
                .NotEmpty()
                    .WithMessage("PlaylistId is required")
                .MustAsync(unitOfWork.Playlists.ExistsAsync)
                    .WithMessage("Playlist not found");
        }
    }
}
