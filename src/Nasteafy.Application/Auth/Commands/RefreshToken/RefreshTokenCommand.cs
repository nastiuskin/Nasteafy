using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Helpers;
using Nasteafy.Domain.Entities.Users;
using System.Security.Claims;

namespace Nasteafy.Application.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<Result<string>>, ITransactionalCommand;

    public class RefreshTokenCommandHandler(
        IJwtTokenService jwtTokenService,
        UserManager<User> userManager,
        IUnitOfWork unitOfWork)
            : IRequestHandler<RefreshTokenCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            // You can pass token
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken != null && u.RefreshToken.Token == request.RefreshToken);

            if (user is null)
                return Result.Fail("Invalid refresh token").Log<AuthenticationService>();

            if (user.RefreshToken!.IsExpired)
                return Result.Fail("Refresh token expired").Log<AuthenticationService>();

            var roles = await userManager.GetRolesAsync(user);

            var subscription = await unitOfWork.Subscriptions.GetActiveSubscriptionAsync(user.Id, ct);

            var claims = new List<Claim>
            {
                new Claim(ClaimsConstants.UserId, user.Id.ToString()),
                // Same empty email comment and in Login command
                new Claim(ClaimsConstants.Email, user.Email ?? string.Empty),
                // I also see in register command you add a free subscription, so can a user have no subscription? Maybe there is not need to pass string.Empty here
                new Claim(ClaimsConstants.SubscriptionType, subscription?.Subscription?.Type.Name ?? string.Empty)
            };

            // You can simplify it as in Login command comment
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimsConstants.Role, role));
            }

            var newAccessToken = jwtTokenService.GenerateAccessToken(claims);

            return Result.Ok(newAccessToken);
        }
    }
}