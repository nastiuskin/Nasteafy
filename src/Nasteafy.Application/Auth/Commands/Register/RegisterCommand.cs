using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Helpers;
using Nasteafy.Domain.Entities.Subscriptions;
using Nasteafy.Domain.Entities.Users;
using Nasteafy.Extensions;

namespace Nasteafy.Application.Auth.Commands.Register
{
    public record RegisterCommand(string Email, string Password) : IRequest<Result>, ITransactionalCommand;

    public class RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        IUserManager userManager,
        IDateTimeService dateTimeService)
        : IRequestHandler<RegisterCommand, Result>
    {
        public async Task<Result> Handle(RegisterCommand request, CancellationToken ct)
        {
            var user = new User
            {
                Email = request.Email,
                UserName = request.Email,
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.IsSuccess)
            {
                return Result.Fail(result.Errors.ToErrorMessage());
            }

            var roleAssignResult = await userManager.AddToRoleAsync(user, UserRole.Guest.ToString());
            if (!roleAssignResult.IsSuccess)
            {
                return Result.Fail(roleAssignResult.Errors.ToErrorMessage());
            }

            var subscription = await unitOfWork.Subscriptions.GetByTypeAsync(SubscriptionType.Free, ct);
            if (subscription is not null)
            {
                var userSubscription = new UserSubscription
                {
                    UserId = user.Id,
                    SubscriptionId = subscription.Id,
                    StartDate = dateTimeService.UtcNow,
                    EndDate = dateTimeService.UtcNow.AddDays(subscription.DurationInDays),
                };

                user.UserSubscriptions.Add(userSubscription);
            }

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }
}

