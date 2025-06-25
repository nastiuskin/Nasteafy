using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Albums.Queries.GetByArtistId
{
    public class GetByArtistIdQueryValidator : AbstractValidator<GetAlbumsByArtistIdQuery>
    {
        public GetByArtistIdQueryValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.ArtistId)
               .NotEmpty()
               .WithMessage("ArtistId should not be empty.")
               .MustAsync(unitOfWork.Artists.ExistsAsync)
               .WithMessage("Artist with given Id does not exist.");
        }
    }
}
