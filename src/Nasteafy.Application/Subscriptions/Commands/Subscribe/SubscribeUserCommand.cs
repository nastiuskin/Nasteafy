using FluentResults;
using MediatR;
using Nasteafy.Application.Abstractions.Auth;
using Nasteafy.Application.Abstractions.Data;
using Nasteafy.Domain.Entities.Subscriptions;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Subscriptions.Commands
{
    public record SubscribeUserCommand(Guid SubscriptionId)
        : IRequest<Result>;


    public class SubscribeUserCommandHandler(IUnitOfWork unitOfWork, IUserIdProvider userProvider)
        : IRequestHandler<SubscribeUserCommand, Result>
    {
        public async Task<Result> Handle(SubscribeUserCommand command, CancellationToken ct)
        {
            var userId = userProvider.GetUserId();

            if (userId == null  || userId == Guid.Empty)
                return Result.Fail("UserId not found");

            var user = await unitOfWork.Users
                .GetWithSubscriptionsAsync(userId.Value, ct);

            var subscription = await unitOfWork.Subscriptions.GetByIdAsync(command.SubscriptionId, ct);

            if (subscription!.Type == SubscriptionType.Trial)
            {
                bool alreadyActivated = user!.UserSubscriptions
                    .Any(us => us.Subscription.Type == SubscriptionType.Trial);

                if (alreadyActivated)
                {
                    return Result.Fail("Trial subscription can be activated only once.");
                }
            }

            if (subscription.Type == SubscriptionType.Artist)
            {
                var artist = new Artist
                {
                    UserId = user!.Id,
                    Name = user!.Email!,
                    AlbumArtists = [],
                    ArtistTracks = [],
                };
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

