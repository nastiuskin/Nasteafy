using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;
using System.Text.Json.Serialization;

namespace Nasteafy.Application.Artists.Commands.Update
{
    public class UpdateArtistCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid ArtistId { get; set; }
        public string? Name { get; set; }
        public IFormFile? AvatarFile { get; set; }
    }

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
                artist!.Name = request.Name;

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
