using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;

namespace Nasteafy.Application.Users.Queries.GetById;

public record GetUserProfileQuery : IRequest<Result<GetUserReponse?>>;

public class GetUserProfileQueryHandler
    : IRequestHandler<GetUserProfileQuery, Result<GetUserReponse?>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly IUserIdProvider _userIdProvider;

    public GetUserProfileQueryHandler(IUnitOfWork unitOfWork,
        IFileStorageService fileStorage,
        IUserIdProvider userIdProvider)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorage;
        _userIdProvider = userIdProvider;
    }
    public async Task<Result<GetUserReponse?>> Handle(GetUserProfileQuery request, CancellationToken ct)
    {
        var userId = _userIdProvider.GetUserId();
        if (userId is null || userId == Guid.Empty)
            return Result.Fail("UserId not found");

        var user = await _unitOfWork.Users.GetByIdAsync(userId.Value, ct);
        if (user is null)
            return Result.Fail("User not found");

        string? avatarUrl = null;

        if (!string.IsNullOrEmpty(user.AvatarUrl))
        {
            var result = await _fileStorageService.GetFileUrlAsync(FileType.UserAvatar,
                objectKey: user.AvatarUrl);

            avatarUrl = result.Value;
        }

        return new GetUserReponse(
            user.Email!,
            user.UserName!,
            avatarUrl
        );
    }
}


