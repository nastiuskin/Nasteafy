using FluentResults;
using MediatR;
using Nasteafy.Application.Abstractions.Auth;
using Nasteafy.Application.Abstractions.Data;
using Nasteafy.Domain.Entities.Subscriptions;

namespace Nasteafy.Application.Subscriptions.Commands
{
    public record SubscribeUserCommand(Guid SubscriptionId)
        : IRequest<Result>;


    public class SubscribeUserCommandHandler(IUnitOfWork unitOfWork, IUserProvider userProvider)
        : IRequestHandler<SubscribeUserCommand, Result>
    {
        public async Task<Result> Handle(SubscribeUserCommand command, CancellationToken ct)
        {
            var userId = userProvider.GetUserId();

            var user = await unitOfWork.Users
                .GetWithSubscriptionsAsync(userId, ct);

            var subscription = await unitOfWork.Subscriptions.GetByIdAsync(command.SubscriptionId, ct);

            //if req is trial subscription, check if was activated earlier

            if (subscription!.Type == SubscriptionType.Trial)
            {
                bool alreadyActivated = user!.UserSubscriptions
                    .Any(us => us.Subscription.Type == SubscriptionType.Trial);

                if (alreadyActivated)
                {
                    return Result.Fail("Trial subscription can be activated only once.");
                }
            }

            var now = DateTime.UtcNow;

            foreach (var sub in user!.UserSubscriptions.Where(s => s.EndDate > now))
            {
                sub.EndDate = now;
            }

            var newUserSubscription = new UserSubscription
            {
                UserId = user.Id,
                SubscriptionId = subscription!.Id,
                StartDate = now,
                EndDate = now.AddDays(subscription.DurationInDays)
            };

            user.UserSubscriptions.Add(newUserSubscription);

            await unitOfWork.SaveChangesAsync(ct);

            return Result.Ok();
        }
    }
}

