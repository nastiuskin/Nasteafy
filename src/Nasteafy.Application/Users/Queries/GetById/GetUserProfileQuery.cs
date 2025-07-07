using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;
using Nasteafy.Domain.Entities.Subscriptions;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Application.Users.Queries.GetById;

public record GetUserProfileQuery : IRequest<Result<GetUserResponse?>>;

public class GetUserProfileQueryHandler(
    IUnitOfWork unitOfWork,
    IFileStorageService fileStorageService,
    ICurrentUserProvider userIdProvider,
    UserManager<User> userManager)
        : IRequestHandler<GetUserProfileQuery, Result<GetUserResponse?>>
{
    public async Task<Result<GetUserResponse?>> Handle(GetUserProfileQuery request, CancellationToken ct)
    {
        var userId = userIdProvider.GetUserId();
        if (userId is null || userId == Guid.Empty)
            return Result.Fail("UserId not found").Log<GetUserProfileQuery>();

        var user = await userManager.FindByIdAsync(userId!.ToString()!);
        if (user is null)
            return Result.Fail("User not found").Log<GetUserProfileQuery>();

        var role = (await userManager.GetRolesAsync(user)).FirstOrDefault() ?? UserRole.User.ToString();

        string? avatarUrl = null;

        if (!string.IsNullOrEmpty(user.AvatarUrl))
        {
            var result = await fileStorageService.GetFileUrlAsync(FileType.UserAvatar, objectKey: user.AvatarUrl);
            avatarUrl = result.IsSuccess ? result.Value : null;
        }

        var subscription = await unitOfWork.Subscriptions.GetActiveSubscriptionAsync(userId!.Value, ct);

        return new GetUserResponse(
            user.Email!,
            user.UserName!,
            role,
            avatarUrl,
            subscription?.Subscription.Type.Name ?? SubscriptionType.Free.Name
        );
    }
}


