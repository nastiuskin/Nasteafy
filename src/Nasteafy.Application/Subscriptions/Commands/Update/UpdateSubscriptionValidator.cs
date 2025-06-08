using FluentValidation;
using Nasteafy.Application.Abstractions.Data;

namespace Nasteafy.Application.Subscriptions.Commands.Update
{
    public class UpdateSubscriptionValidator : AbstractValidator<UpdateSubscriptionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateSubscriptionValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Subscription Id is required.")
                .MustAsync(Exist).WithMessage("Subscription with given Id does not exist.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.");

            RuleFor(x => x.DurationInDays)
                .GreaterThan(0).WithMessage("Duration must be greater than zero.");
        }

        private async Task<bool> Exist(Guid id, CancellationToken ct)
        {
            return await _unitOfWork.Subscriptions.ExistsAsync(id, ct);
        }
    }
}
