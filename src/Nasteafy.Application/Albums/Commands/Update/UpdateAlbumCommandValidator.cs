using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Albums.Commands.Update
{
    public class UpdateAlbumCommandValidator : AbstractValidator<UpdateAlbumCommand>
    {
        public UpdateAlbumCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.AlbumId)
                .NotEmpty()
                    .WithMessage("AlbumId is required")
                .MustAsync(unitOfWork.Albums.ExistsAsync)
                    .WithMessage("Album not found");
        }
    }
}
