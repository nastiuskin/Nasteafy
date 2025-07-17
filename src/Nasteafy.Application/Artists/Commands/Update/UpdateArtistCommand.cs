using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Helpers;
using Nasteafy.Domain;

namespace Nasteafy.Application.Artists.Commands.Update
{
    public record UpdateArtistCommand(
         Guid ArtistId,
         string? Name,
         string? Biography,
         IFormFile? AvatarFile) : IRequest<Result>, ITransactionalCommand;

    public class UpdateArtistCommandHandler : IRequestHandler<UpdateArtistCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorage;

        public UpdateArtistCommandHandler(
            IUnitOfWork unitOfWork,
            IFileStorageService fileStorage)
        {
            _unitOfWork = unitOfWork;
            _fileStorage = fileStorage;
        }

        public async Task<Result> Handle(UpdateArtistCommand request, CancellationToken ct)
        {
            var artist = await _unitOfWork.Artists.GetByIdAsync(request.ArtistId, ct);

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                artist!.Name = request.Name;
            }

            artist!.Biography = request.Biography;
              
            if (request.AvatarFile != null && request?.AvatarFile?.Length > 0)
            {
                await using var stream = request.AvatarFile.OpenReadStream();

                var result = await _fileStorage.UploadFileAsync(
                    stream,
                    request.AvatarFile.FileName,
                    request.AvatarFile.ContentType,
                    FileType.UserAvatar);

                artist!.AvatarUrl = result.Value;
            }

            await _unitOfWork.Artists.UpdateAsync(artist!, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Ok();
        }
    }
}
