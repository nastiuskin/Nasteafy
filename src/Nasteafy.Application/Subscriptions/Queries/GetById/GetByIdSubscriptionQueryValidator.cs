using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Subscriptions.Queries.GetById
{
    public class GetByIdSubscriptionQueryValidator : AbstractValidator<GetSubscriptionByIdQuery>
    {
        public GetByIdSubscriptionQueryValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.SubscriptionId)
               .NotEmpty().WithMessage("Subscription Id should not be empty.")
               .MustAsync(unitOfWork.Subscriptions.ExistsAsync)
               .WithMessage("Subscription with given Id does not exist.");
        }
    }
}
