using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Albums.Queries.GetById
{
    public class GetAlbumByIdQueryValidator : AbstractValidator<GetAlbumByIdQuery>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAlbumByIdQueryValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.AlbumId)
               .NotEmpty().WithMessage("Album Id should not be empty.")
               .MustAsync(_unitOfWork.Albums.ExistsAsync)
               .WithMessage("Album with given Id does not exist.");
        }
    }
}
