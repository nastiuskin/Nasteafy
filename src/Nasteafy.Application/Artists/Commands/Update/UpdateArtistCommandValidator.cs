using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Artists.Commands.Update
{
    public class UpdateArtistCommandValidator : AbstractValidator<UpdateArtistCommand>
    {
        public UpdateArtistCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.ArtistId)
                .NotEmpty().WithMessage("ArtistId is required")
                .MustAsync(unitOfWork.Artists.ExistsAsync).WithMessage("Artist not found");
        }
    }
}
