using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain.Entities.Subscriptions;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Application.Auth.Commands.Register
{
    public record RegisterCommand(string Email, string Password) : IRequest<Result>;

    public class RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        UserManager<User> userManager)
        : IRequestHandler<RegisterCommand, Result>
    {
        public async Task<Result> Handle(RegisterCommand request, CancellationToken ct)
        {
            var existingUser = await userManager.FindByEmailAsync(request.Email);

            if (existingUser is not null)
                return Result.Fail("User already exists").Log<AuthenticationService>();

            var user = new User
            {
                Email = request.Email,
                UserName =  request.Email,
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return Result.Fail(string.Join(", ", result.Errors.Select(e => e.Description)))
                   .Log<AuthenticationService>();
            }

            var roleAssignResult = await userManager.AddToRoleAsync(user, UserRole.User.ToString());
            if (!roleAssignResult.Succeeded)
            {
                return Result.Fail(string.Join(", ", roleAssignResult.Errors.Select(e => e.Description)))
                    .Log<AuthenticationService>();
            }

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

