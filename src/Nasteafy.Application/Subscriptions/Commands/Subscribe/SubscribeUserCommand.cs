using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Helpers;
using Nasteafy.Domain.Entities.Subscriptions;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Application.Subscriptions.Commands
{
    public record SubscribeUserCommand(Guid SubscriptionId): IRequest<Result>, ITransactionalCommand;

    public class SubscribeUserCommandHandler(IUnitOfWork unitOfWork,
        ICurrentUserProvider userProvider,
        UserManager<User> userManager, 
        IDateTimeService dateTimeService)
        : IRequestHandler<SubscribeUserCommand, Result>
    {
        public async Task<Result> Handle(SubscribeUserCommand command, CancellationToken ct)
        {
            var userId = userProvider.GetUserId();

            if (userId == null  || userId == Guid.Empty)
            {
                return Result.Fail("UserId not found").Log<SubscribeUserCommandHandler>();
            }               

            var user = await unitOfWork.Users.GetByIdWithSubscriptionsAsync(userId, ct);

            var subscription = await unitOfWork.Subscriptions.GetByIdAsync(command.SubscriptionId, ct);

            if (subscription!.Type == SubscriptionType.Trial)
            {
                bool alreadyActivated = user!.UserSubscriptions.Any(us => us.Subscription.Type == SubscriptionType.Trial);

                if (alreadyActivated)
                {
                    return Result.Fail("Trial subscription can be activated only once.").Log<SubscribeUserCommandHandler>();
                }                    
            }

            if (subscription.Type == SubscriptionType.Artist)
            {
                var userWithRoles = await userManager.FindByIdAsync(userId.ToString()!);
                if (userWithRoles is not null)
                {
                    var currentRoles = await userManager.GetRolesAsync(userWithRoles);
                    if (!currentRoles.Contains(UserRole.Artist.ToString()))
                    {
                        if (currentRoles.Any())
                        {
                            await userManager.RemoveFromRolesAsync(userWithRoles, currentRoles);
                        }                           

                        await userManager.AddToRoleAsync(userWithRoles, UserRole.Artist.ToString());
                    }
                }

                var alreadyArtist = await unitOfWork.Artists.ExistsByUserIdAsync(user.Id, ct);
                if (!alreadyArtist)
                {
                    var artist = new Artist
                    {
                        UserId = user.Id,
                        Name = user.UserName!,
                        AvatarUrl = user.AvatarUrl,
                    };
                    await unitOfWork.Artists.AddAsync(artist, ct);
                }
            }

            var now = dateTimeService.UtcNow;

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


