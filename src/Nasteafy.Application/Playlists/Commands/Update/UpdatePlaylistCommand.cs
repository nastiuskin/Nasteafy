using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;

namespace Nasteafy.Application.Playlists.Commands.Update
{
    public record UpdatePlaylistCommand(Guid PlaylistId, string? Title, IFormFile? CoverFile) : IRequest<Result>;

    public class UpdatePlaylistCommandHandler : IRequestHandler<UpdatePlaylistCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorage;

        public UpdatePlaylistCommandHandler(
            IUnitOfWork unitOfWork,
            IFileStorageService fileStorage)
        {
            _unitOfWork = unitOfWork;
            _fileStorage = fileStorage;
        }

        public async Task<Result> Handle(UpdatePlaylistCommand request, CancellationToken ct)
        {
            var playlist = await _unitOfWork.Playlists.GetByIdAsync(request.PlaylistId, ct);
            if (playlist == null) return Result.Fail("Playlist not found");

            if (!string.IsNullOrWhiteSpace(request.Title))
                playlist.Title = request.Title;

            if (request.CoverFile != null || request?.CoverFile?.Length > 0)
            {
                await using var stream = request.CoverFile.OpenReadStream();

                var result = await _fileStorage.UploadFileAsync(
                    stream,
                    request.CoverFile.FileName,
                    request.CoverFile.ContentType,
                    FileType.PlaylistCover);

                playlist.CoverUrl = result.Value;

                await _unitOfWork.Playlists.UpdateAsync(playlist, ct);
            }

            await _unitOfWork.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }
}
