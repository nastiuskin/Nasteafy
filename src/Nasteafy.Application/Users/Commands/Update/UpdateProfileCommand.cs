using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;

namespace Nasteafy.Application.Users.Commands.Update
{
    public record UpdateProfileCommand(string? Email, string? UserName, IFormFile? AvatarFile) : IRequest<Result>;

    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserIdProvider _userProvider;
        private readonly IFileStorageService _fileStorage;

        public UpdateProfileCommandHandler(
            IUnitOfWork unitOfWork,
            IUserIdProvider userProvider,
            IFileStorageService fileStorage)
        {
            _unitOfWork = unitOfWork;
            _userProvider = userProvider;
            _fileStorage = fileStorage;
        }

        public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken ct)
        {
            var userId = _userProvider.GetUserId();
            if (userId == null  || userId == Guid.Empty)
                return Result.Fail("UserId not found");

            var user = await _unitOfWork.Users.GetByIdAsync(userId.Value, ct);
            if (user == null) return Result.Fail("User not found");

            if (!string.IsNullOrWhiteSpace(request.Email))
                user.Email = request.Email;

            if (!string.IsNullOrEmpty(request.UserName))
                user.UserName = request.UserName;

            if (request.AvatarFile != null || request?.AvatarFile?.Length > 0)
            {
                await using var stream = request.AvatarFile.OpenReadStream();

                var result = await _fileStorage.UploadFileAsync(
                    stream,
                    request.AvatarFile.FileName,
                    request.AvatarFile.ContentType,
                    FileType.UserAvatar);

                user.AvatarUrl = result.Value;

                await _unitOfWork.Users.UpdateAsync(user, ct);
            }

            await _unitOfWork.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }
}
