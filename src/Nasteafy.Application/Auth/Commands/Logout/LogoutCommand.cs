using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Helpers;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Application.Auth.Commands.Logout
{
    public record LogoutCommand : IRequest<Result>, ITransactionalCommand;

    public class LogoutCommandHandler(
        ICurrentUserProvider userProvider,
        UserManager<User> userManager) : IRequestHandler<LogoutCommand, Result>
    {
        public async Task<Result> Handle(LogoutCommand command, CancellationToken ct)
        {
            var userId = userProvider.GetUserId();

            // You can pass token 
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return Result.Fail("Unauthorized").Log<AuthenticationService>();
            }

            user.RefreshToken = null;
            await userManager.UpdateAsync(user);

            return Result.Ok();
        }
    }
}
