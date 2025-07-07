using Nasteafy.Domain.Entities.Subscriptions;

namespace Nasteafy.Application.Users.Queries.GetById
{
    public record GetUserResponse(string Email,
        string UserName,
        string UserRole,
        string? AvatarUrl,
        string? SubscriptionType);
}
