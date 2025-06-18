using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Albums.Commands.Delete
{
    public class DeleteAlbumCommandValidator : AbstractValidator<DeleteAlbumCommand>
    {
        public DeleteAlbumCommandValidator(IUnitOfWork unitOfWork)
        {

            RuleFor(x => x.AlbumId)
                .NotEmpty()
                .WithMessage("Album ID must not be empty.")
                .MustAsync(unitOfWork.Albums.ExistsAsync)
                .WithMessage("Album does not exist.");
        }
    }

}
