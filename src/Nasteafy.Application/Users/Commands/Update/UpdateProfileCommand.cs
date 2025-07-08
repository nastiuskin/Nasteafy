using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Helpers;
using Nasteafy.Domain;

namespace Nasteafy.Application.Users.Commands.Update
{
    public class UpdateProfileCommand : IRequest<Result>, ITransactionalCommand
    {
        public string? Email { get; init; }
        public string? UserName { get; init; }
        public IFormFile? AvatarFile { get; init; }
    }

    public class UpdateProfileCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserProvider userProvider,
        IFileStorageService fileStorage)
            : IRequestHandler<UpdateProfileCommand, Result>
    {
        public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken ct)
        {
            var userId = userProvider.GetUserId();
            if (userId == null  || userId == Guid.Empty)
            {
                return Result.Fail("UserId not found").Log<UpdateProfileCommandHandler>();
            }                

            var user = await unitOfWork.Users.GetByIdAsync(userId, ct);
            if (user == null)
            {
                return Result.Fail("User not found").Log<UpdateProfileCommandHandler>();
            }                

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                user.Email = request.Email;
            }                

            if (!string.IsNullOrEmpty(request.UserName))
            {
                user.UserName = request.UserName;
            }                

            if (request.AvatarFile != null && request?.AvatarFile?.Length > 0)
            {
                await using var stream = request.AvatarFile.OpenReadStream();

                var result = await fileStorage.UploadFileAsync(
                    stream,
                    request.AvatarFile.FileName,
                    request.AvatarFile.ContentType,
                    FileType.UserAvatar);

                user.AvatarUrl = result.IsSuccess ? result.Value : null;
            }

            await unitOfWork.Users.UpdateAsync(user, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Ok();
        }
    }
}
