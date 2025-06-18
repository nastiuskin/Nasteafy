using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Tracks.Queries.GetByPlaylistId
{
    public class GetTracksByPlaylistIdQueryValidator : AbstractValidator<GetTracksByPlaylistIdQuery>
    {
        public GetTracksByPlaylistIdQueryValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.PlaylistId)
                .NotEmpty()
                    .WithMessage("PlaylistId is required")
                .MustAsync(unitOfWork.Playlists.ExistsAsync)
                    .WithMessage("Playlist not found.");
        }
    }
}
