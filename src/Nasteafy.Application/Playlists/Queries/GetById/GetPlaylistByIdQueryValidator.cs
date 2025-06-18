using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Playlists.Queries.GetById;

namespace Nasteafy.Application.Playlists.Queries.GetById
{
    public class GetPlaylistByIdQueryValidator : AbstractValidator<GetPlaylistByIdQuery>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPlaylistByIdQueryValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.PlaylistId)
               .NotEmpty().WithMessage("Playlist Id should not be empty.")
               .MustAsync(_unitOfWork.Playlists.ExistsAsync)
               .WithMessage("Playlist with given Id does not exist.");
        }
    }
}
