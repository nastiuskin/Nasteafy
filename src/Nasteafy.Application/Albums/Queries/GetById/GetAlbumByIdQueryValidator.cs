using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Albums.Queries.GetById
{
    public class GetAlbumByIdQueryValidator : AbstractValidator<GetAlbumByIdQuery>
    {

        public GetAlbumByIdQueryValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.AlbumId)
               .NotEmpty().WithMessage("Album Id should not be empty.")
               .MustAsync(unitOfWork.Albums.ExistsAsync)
               .WithMessage("Album with given Id does not exist.");
        }
    }
}
