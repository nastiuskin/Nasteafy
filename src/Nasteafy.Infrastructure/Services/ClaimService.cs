using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain.Entities.Subscriptions;
using Nasteafy.Domain.Entities.Users;
using Nasteafy.Infrastructure.Constants;
using System.Security.Claims;

namespace Nasteafy.Infrastructure.Services
{
    public class ClaimService(IUserManager userManager, IUnitOfWork unitOfWork) : IClaimService
    {
        public async Task<List<Claim>> GenerateClaimsAsync(User user, CancellationToken ct)
        {
            var subscription = await unitOfWork.Subscriptions.GetActiveSubscriptionAsync(user.Id, ct);

            var roles = await userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimsConstants.UserId, user.Id.ToString()),
                new Claim(ClaimsConstants.Email, user.Email!),
                new Claim(ClaimsConstants.SubscriptionType, subscription?.Subscription?.Type.Name ?? SubscriptionType.Free.Name)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimsConstants.Role, role)));
            return claims;
        }
    }
}
