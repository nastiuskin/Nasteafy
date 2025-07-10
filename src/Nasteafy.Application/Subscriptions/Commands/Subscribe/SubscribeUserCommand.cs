using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

            if (userId == Guid.Empty)
            {
                return Result.Fail("UserId not found").Log<SubscribeUserCommandHandler>();
            }               

            var user = await unitOfWork.Users.GetByIdWithSubscriptionsAsync(userId, ct);
            if(user is null)
            {
                return Result.Fail("User not found").Log<SubscribeUserCommandHandler>();
            }

            var subscription = await unitOfWork.Subscriptions.GetByIdAsync(command.SubscriptionId, ct);

            var existingArtist = await unitOfWork.Artists.GetByIdAsync(user.Id, ct);

            if (subscription!.Type == SubscriptionType.Trial)
            {
                bool alreadyActivated = user!.UserSubscriptions.Any(us => us.Subscription.Type == SubscriptionType.Trial);

                if (alreadyActivated)
                {
                    return Result.Fail("Trial subscription can be activated only once.").Log<SubscribeUserCommandHandler>();
                }                    
            }

            var userWithRoles = await userManager.FindByIdAsync(userId.ToString()!);

            if (subscription.Type == SubscriptionType.Artist)
            {
                if (userWithRoles is not null)
                {
                    var currentRoles = await userManager.GetRolesAsync(userWithRoles);
                    if (!currentRoles.Contains(UserRole.Artist.ToString()))
                    {
                        await userManager.AddToRoleAsync(userWithRoles, UserRole.Artist.ToString());
                    }
                }

                if (existingArtist is null)
                {
                    var artist = new Artist
                    {
                        UserId = user.Id,
                        Name = user.UserName!,
                        AvatarUrl = user.AvatarUrl,
                    };

                    await unitOfWork.Artists.AddAsync(artist, ct);
                }
                else
                {
                    existingArtist.Name = user.UserName!;
                    existingArtist.AvatarUrl = user.AvatarUrl;
                }
            }
            else
            {
                if (userWithRoles is not null)
                {
                    var currentRoles = await userManager.GetRolesAsync(userWithRoles);
                    if (currentRoles.Contains(UserRole.Artist.ToString()))
                    {
                        await userManager.RemoveFromRoleAsync(userWithRoles, UserRole.Artist.ToString());
                    }
                }
            }

            var now = dateTimeService.UtcNow;

            foreach (var sub in user.UserSubscriptions.Where(s => s.EndDate > now && s.SubscriptionId != subscription.Id))
            {
                sub.EndDate = now;
            }

            var existingSub = user.UserSubscriptions.FirstOrDefault(s => s.SubscriptionId == subscription.Id);

            if (existingSub is not null)
            {
                existingSub.StartDate = now;
                existingSub.EndDate = now.AddDays(subscription.DurationInDays);
            }
            else
            {
                var newUserSubscription = new UserSubscription
                {
                    UserId = user.Id,
                    SubscriptionId = subscription.Id,
                    StartDate = now,
                    EndDate = now.AddDays(subscription.DurationInDays)
                };

               user.UserSubscriptions.Add(newUserSubscription);
            }

            await unitOfWork.SaveChangesAsync(ct);

            return Result.Ok();
        }
    }
}


