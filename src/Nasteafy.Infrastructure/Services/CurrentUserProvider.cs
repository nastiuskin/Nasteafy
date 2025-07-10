using Microsoft.AspNetCore.Http;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Infrastructure.Constants;

namespace Nasteafy.Infrastructure.Services
{
    public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
    {
        public Guid GetUserId()
        {
            var claim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimsConstants.UserId);
            if (Guid.TryParse(claim?.Value, out var id))
                return id;

            return Guid.Empty;
        }
    }
}
