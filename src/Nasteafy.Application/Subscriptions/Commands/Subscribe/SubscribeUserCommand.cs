using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Helpers;
using Nasteafy.Domain.Entities.Subscriptions;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Application.Subscriptions.Commands
{
    public record SubscribeUserCommand(Guid SubscriptionId) : IRequest<Result>, ITransactionalCommand;

    public class SubscribeUserCommandHandler(IUnitOfWork unitOfWork,
        ICurrentUserProvider userProvider,
        IUserManager userManager,
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
            if (user is null)
            {
                return Result.Fail("User not found").Log<SubscribeUserCommandHandler>();
            }

            var subscription = await unitOfWork.Subscriptions.GetByIdAsync(command.SubscriptionId, ct);
            var userRoles = await userManager.GetRolesAsync(user);

            var isTrialAlreadyActivated = user.UserSubscriptions.Any(us => us.Subscription.Type == SubscriptionType.Trial);
            if (subscription!.Type == SubscriptionType.Trial && isTrialAlreadyActivated)
            {
                return Result.Fail("Trial subscription can be activated only once.").Log<SubscribeUserCommandHandler>();
            }

            if (subscription.Type == SubscriptionType.Artist)
            {
                await AddOrSyncArtistEntityAsync(user, userRoles, ct);
            }
            else
            {
                await RemoveArtistRoleIfExistsAsync(user, userRoles, ct);
            }

            DeactivateOtherSubscriptions(user, subscription.Id);
            UpdateOrAddSubscription(user, subscription);

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Ok();
        }

        private void DeactivateOtherSubscriptions(User user, Guid activeSubscriptionId)
        {
            var now = dateTimeService.UtcNow;
            foreach (var sub in user.UserSubscriptions.Where(s => s.EndDate > now && s.SubscriptionId != activeSubscriptionId))
            {
                sub.EndDate = now;
            }
        }

        private async Task RemoveArtistRoleIfExistsAsync(User user, IList<string> roles, CancellationToken ct)
        {
            var artistRole = UserRole.Artist.ToString();
            if (roles.Contains(artistRole))
            {
                await userManager.RemoveRoleAsync(user, artistRole);
            }
        }

        private void UpdateOrAddSubscription(User user, Subscription subscription)
        {
            var now = dateTimeService.UtcNow;
            var endDate = now.AddDays(subscription.DurationInDays);

            var existing = user.UserSubscriptions.FirstOrDefault(s => s.SubscriptionId == subscription.Id);

            if (existing is not null)
            {
                existing.StartDate = now;
                existing.EndDate = endDate;
            }
            else
            {
                user.UserSubscriptions.Add(new UserSubscription
                {
                    UserId = user.Id,
                    SubscriptionId = subscription.Id,
                    StartDate = now,
                    EndDate = endDate
                });
            }
        }

        private async Task AddOrSyncArtistEntityAsync(User user, IList<string> roles, CancellationToken ct)
        {
            var artistRole = UserRole.Artist.ToString();

            if (!roles.Contains(artistRole))
            {
                await userManager.AddToRoleAsync(user, artistRole);
            }

            var existingArtist = await unitOfWork.Artists.GetByUserIdAsync(user.Id, ct);

            if (existingArtist is null)
            {
                var newArtist = new Artist
                {
                    UserId = user.Id,
                    Name = user.UserName!,
                    AvatarUrl = user.AvatarUrl
                };

                await unitOfWork.Artists.AddAsync(newArtist, ct);
            }
            else
            {
                existingArtist.Name = user.UserName!;
                existingArtist.AvatarUrl = user.AvatarUrl;

                await unitOfWork.Artists.UpdateAsync(existingArtist, ct);
            }
        }
    }
}


