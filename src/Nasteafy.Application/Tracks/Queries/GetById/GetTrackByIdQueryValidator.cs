using FluentValidation;
using Nasteafy.Application.Tracks.Queries.GetById;
using Nasteafy.Application.Common.Abstractions.Data;

public class GetTrackByIdQueryValidator : AbstractValidator<GetTrackByIdQuery>
{
    public GetTrackByIdQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.TrackId)
            .NotEmpty()
                .WithMessage("Track ID must not be empty.")
            .MustAsync(unitOfWork.Tracks.ExistsAsync)
                .WithMessage("Track not found.");
    }
}
