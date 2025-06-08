using FluentValidation;
using Nasteafy.Application.Abstractions.Data;

namespace Nasteafy.Application.Subscriptions.Queries.GetById
{
    public class GetByIdSubscriptionQueryValidator : AbstractValidator<GetByIdSubscriptionQuery>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdSubscriptionQueryValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.SubscriptionId)
               .NotEmpty().WithMessage("Subscription Id should not be empty.")
               .MustAsync(SubscriptionExists).WithMessage("Subscription with given Id does not exist.");
        }
        private async Task<bool> SubscriptionExists(Guid subscriptionId, CancellationToken ct)
        {
            return await _unitOfWork.Subscriptions.ExistsAsync(subscriptionId, ct);
        }
    }
}
