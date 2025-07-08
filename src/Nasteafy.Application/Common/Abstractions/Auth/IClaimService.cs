using Nasteafy.Domain.Entities.Users;
using System.Security.Claims;

namespace Nasteafy.Application.Common.Abstractions.Auth
{
    public interface IClaimService
    {
        Task<List<Claim>> GenerateClaimsAsync(User user, CancellationToken ct);
    }
}
