using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Albums.Commands.Rate
{
    public class RateAlbumCommandValidator : AbstractValidator<RateAlbumCommand>
    {
        public RateAlbumCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.AlbumId)
                .NotEmpty().WithMessage("Album ID is required.")
                .MustAsync(unitOfWork.Albums.ExistsAsync).WithMessage("Album not found");
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
        }
    }
}
