using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Tracks.Queries.GetById;

public class GetTrackByIdQueryValidator : AbstractValidator<GetTrackByIdQuery>
{
    public GetTrackByIdQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.TrackId)
            .NotEmpty().WithMessage("Track ID must not be empty.")
            .MustAsync(unitOfWork.Tracks.ExistsAsync).WithMessage("Track not found.");
    }
}
