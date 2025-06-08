using Microsoft.AspNetCore.Http;
using Nasteafy.Application.Abstractions.Auth;
using Nasteafy.Infrastructure.Constants;


namespace Nasteafy.Infrastructure.Services
{
    public class UserProvider(IHttpContextAccessor httpContextAccessor) : IUserProvider
    {
        public Guid GetUserId()
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimsConstants.UserId);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            return userId;
        }
    }
}
