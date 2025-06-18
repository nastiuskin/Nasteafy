using FluentValidation;
using Nasteafy.Application.Tracks.Queries.GetByAlbumId;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Tracks.Validators
{
    public class GetTracksByAlbumIdQueryValidator : AbstractValidator<GetTracksByAlbumIdQuery>
    {
        public GetTracksByAlbumIdQueryValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.AlbumId)
                .NotEmpty().WithMessage("Album ID must not be empty.")
                .MustAsync(unitOfWork.Albums.ExistsAsync)
                .WithMessage("Album does not exist.");
        }
    }
}
