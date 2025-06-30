using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;

namespace Nasteafy.Application.Users.Queries.GetById;

public record GetUserProfileQuery : IRequest<Result<GetUserResponse?>>;

public class GetUserProfileQueryHandler(
    IUnitOfWork unitOfWork,
    IFileStorageService fileStorageService,
    ICurrentUserProvider userIdProvider)
        : IRequestHandler<GetUserProfileQuery, Result<GetUserResponse?>>
{
    public async Task<Result<GetUserResponse?>> Handle(GetUserProfileQuery request, CancellationToken ct)
    {
        var userId = userIdProvider.GetUserId();
        if (userId is null || userId == Guid.Empty)
            return Result.Fail("UserId not found").Log<GetUserProfileQuery>();

        var user = await unitOfWork.Users.GetByIdWithSubscriptionsAsync(userId.Value, ct);
        if (user is null)
            return Result.Fail("User not found").Log<GetUserProfileQuery>();

        string? avatarUrl = null;

        if (!string.IsNullOrEmpty(user.AvatarUrl))
        {
            var result = await fileStorageService.GetFileUrlAsync(FileType.UserAvatar, objectKey: user.AvatarUrl);
            avatarUrl = result.IsSuccess ? result.Value : null;
        }

        var subscription = user.UserSubscriptions
            .Where(x => x.EndDate >= DateTime.UtcNow)
            .Select(x => x.Subscription)
            .FirstOrDefault();

        return new GetUserResponse(
            user.Email!,
            user.UserName!,
            avatarUrl,
            subscription?.Type
        );
    }
}


