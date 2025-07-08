using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Helpers;
using Nasteafy.Domain;
using System.Text.Json.Serialization;

namespace Nasteafy.Application.Albums.Commands.Update
{
    public class UpdateAlbumCommand : IRequest<Result>, ITransactionalCommand
    {
        [JsonIgnore]
        public Guid AlbumId { get; set; }
        public IFormFile? CoverFile { get; init; }
        public DateTime ReleaseDate { get; init; }
        public string? Title { get; init; } 
    }

    public class UpdateAlbumCommandHandler : IRequestHandler<UpdateAlbumCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorage;

        public UpdateAlbumCommandHandler(
            IUnitOfWork unitOfWork,
            IFileStorageService fileStorage)
        {
            _unitOfWork = unitOfWork;
            _fileStorage = fileStorage;
        }

        public async Task<Result> Handle(UpdateAlbumCommand request, CancellationToken ct)
        {
            var album = await _unitOfWork.Albums
                .GetByIdAsync(request.AlbumId, ct);

            if (!string.IsNullOrWhiteSpace(request.Title))
                album!.Title = request.Title;

            if (request.CoverFile != null && request?.CoverFile?.Length > 0)
            {
                await using var stream = request.CoverFile.OpenReadStream();

                var result = await _fileStorage.UploadFileAsync(
                    stream,
                    request.CoverFile.FileName,
                    request.CoverFile.ContentType,
                    FileType.AlbumCover);

                album!.CoverUrl = result.Value;
            }

            await _unitOfWork.Albums.UpdateAsync(album!, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }
}

