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

            //Since you do not reuse existingUser, you can move this check and FindByEmailAsync into validator.
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
                /*
                 It is better to move this string join in extension methos and reuse it.
                  public static string ToErrorsString(this IEnumerable<string> errors)
                    {
                        return string.Join(", ", errors);
                    }

                 Then you can use it like return Result.Fail(roleAssignResult.Errors.Select(e => e.Description).ToErrorsString());
                 It is clearer and you standardize the errors string.
                 */
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
                    // It is better to wrap DateTime into a simple interface such as 
                    /*
                     public interface IDateTimeService
                        {
                            DateTime UtcNow { get; }
                        }

                        public class DateTimeService : IDateTimeService
                        {
                            public DateTime UtcNow => DateTime.UtcNow;
                        }
                     */
                    // Using DateTime directly is easier and is a direct approach, not an issue in most cases, but becomes difficult to unit test since it is a static class
                    // and returns actual utc date on the time of the test. With interface you can mock it and return a specific date, test different scenarios and avoid
                    // issues where your unit tests are running in a different timezone and start failing because of that.
                    // also it standardizes usage of UTC only, where with DateTime you can use just .Now() accidentally. 
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

