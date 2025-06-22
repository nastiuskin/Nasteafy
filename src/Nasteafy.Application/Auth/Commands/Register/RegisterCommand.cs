using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain.Entities.Subscriptions;

namespace Nasteafy.Application.Auth.Commands.Register
{
    public record RegisterCommand(string Email, string Password) : IRequest<Result>;

    public class RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        IAuthenticationService authenticationService)
        : IRequestHandler<RegisterCommand, Result>
    {
        public async Task<Result> Handle(RegisterCommand request, CancellationToken ct)
        {
            var registerResult = await authenticationService.RegisterAsync(request.Email, request.Password);

            if (registerResult.IsFailed)
                return Result.Fail(registerResult.Errors);

            var user = await unitOfWork.Users.GetByIdAsync(registerResult.Value, ct);
            if (user is null)
                return Result.Fail("User not found.");

            var subscription = await unitOfWork.Subscriptions.GetByTypeAsync(SubscriptionType.Free, ct);
            if (subscription != null)
            {
                var userSubscription = new UserSubscription
                {
                    UserId = user.Id,
                    SubscriptionId = subscription.Id,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(subscription.DurationInDays),
                };

                user.UserSubscriptions.Add(userSubscription);
            }

            await unitOfWork.SaveChangesAsync(ct);

            return Result.Ok();
        }
    }
}

