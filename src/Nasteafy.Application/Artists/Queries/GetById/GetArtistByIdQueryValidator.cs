using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Artists.Queries.GetById
{
    public class GetArtistByIdQueryValidator : AbstractValidator<GetArtistByIdQuery>
    {
        public GetArtistByIdQueryValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.ArtistId)
                .NotEmpty().WithMessage("Artist Id must not be empty.")
                .MustAsync(unitOfWork.Artists.ExistsAsync)
                .WithMessage("Artist not found.");
        }
    }

}
