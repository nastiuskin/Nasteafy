using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Helpers;

namespace Nasteafy.Application.Auth.Commands.Logout
{
    public record LogoutCommand : IRequest<Result>, ITransactionalCommand;

    public class LogoutCommandHandler(
        ICurrentUserProvider userProvider,
        IUserManager userManager,
        IUnitOfWork unitOfWork) : IRequestHandler<LogoutCommand, Result>
    {
        public async Task<Result> Handle(LogoutCommand command, CancellationToken ct)
        {
            var userId = userProvider.GetUserId();

            var user = await unitOfWork.Users.GetByIdAsync(userId, ct);

            if (user == null)
            {
                return Result.Fail("User not authenticated").Log<LogoutCommandHandler>();
            }

            user.RefreshToken = null;
            await userManager.UpdateAsync(user);

            return Result.Ok();
        }
    }
}
