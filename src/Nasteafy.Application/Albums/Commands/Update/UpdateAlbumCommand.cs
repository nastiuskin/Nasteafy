using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nasteafy.Application.Artists.Commands.Delete;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;

namespace Nasteafy.Application.Albums.Commands.Update
{
    public record UpdateAlbumCommand(Guid AlbumId, string? Title, IFormFile? CoverFile) : IRequest<Result>;

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

            if (album == null) 
                return Result.Fail("Album not found")
                    .LogIfFailed<UpdateAlbumCommandHandler>();

            if (!string.IsNullOrWhiteSpace(request.Title))
                album.Title = request.Title;

            if (request.CoverFile != null && request?.CoverFile?.Length > 0)
            {
                await using var stream = request.CoverFile.OpenReadStream();

                var result = await _fileStorage.UploadFileAsync(
                    stream,
                    request.CoverFile.FileName,
                    request.CoverFile.ContentType,
                    FileType.AlbumCover);

                album.CoverUrl = result.Value;
            }

            await _unitOfWork.Albums.UpdateAsync(album, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }
}

