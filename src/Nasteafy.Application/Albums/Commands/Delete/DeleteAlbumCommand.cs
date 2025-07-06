using FluentResults;
using MediatR;
using Nasteafy.Application.Artists.Commands.Delete;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;

namespace Nasteafy.Application.Albums.Commands.Delete
{
    public record DeleteAlbumCommand(Guid AlbumId) : IRequest<Result>;

    public class DeletePlaylistCommandHandler(IUnitOfWork unitOfWork, IFileStorageService fileStorageService)
        : IRequestHandler<DeleteAlbumCommand, Result>
    {
        public async Task<Result> Handle(DeleteAlbumCommand request, CancellationToken ct)
        {
            var album = await unitOfWork.Albums.GetByIdAsync(request.AlbumId, ct);

            if (!string.IsNullOrEmpty(album!.CoverUrl))
                await fileStorageService.DeleteFileAsync(FileType.AlbumCover, album.CoverUrl);

            unitOfWork.Albums.Delete(album, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Ok();
        }
    }
}
