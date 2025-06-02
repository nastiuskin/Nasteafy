using FluentResults;
using MediatR;
using Nasteafy.Application.Abstractions.Data;
using Nasteafy.Domain.Entities.Subscriptions;

namespace Nasteafy.Application.Subscriptions.Commands
{
    public record ActivateSubscriptionCommand(Guid UserId, Guid SubscriptionId)
        : IRequest<Result>;


    public class ActivateSubscriptionCommandHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<ActivateSubscriptionCommand, Result>
    {
        public async Task<Result> Handle(ActivateSubscriptionCommand command, CancellationToken ct)
        {
            var user = await unitOfWork.Users
                .GetWithSubscriptionsAsync(command.UserId, ct);

            if (user is null)
                return Result.Fail("User not found.");

            var subscription = await unitOfWork.Subscriptions.GetByIdAsync(command.SubscriptionId, ct);

            //if req is trial subscription, check if was activated earlier

            if (subscription is null)
                return Result.Fail("Subscription not found.");

            var now = DateTime.UtcNow;

            foreach (var sub in user.UserSubscriptions.Where(s => s.EndDate > now))
            {
                sub.EndDate = now;
            }

            var newUserSubscription = new UserSubscription
            {
                UserId = user.Id,
                SubscriptionId = subscription.Id,
                StartDate = now,
                EndDate = now.AddDays(subscription.DurationInDays)
            };

            user.UserSubscriptions.Add(newUserSubscription);

            await unitOfWork.SaveChangesAsync(ct); 

            return Result.Ok();
        }
    }
}

