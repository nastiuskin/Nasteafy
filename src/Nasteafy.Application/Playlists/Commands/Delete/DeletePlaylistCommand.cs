using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;

namespace Nasteafy.Application.Playlists.Commands.Delete
{
    public record DeletePlaylistCommand(Guid PlaylistId) : IRequest<Result>;

    public class DeletePlaylistCommandHandler(IUnitOfWork unitOfWork, IFileStorageService fileStorageService)
        : IRequestHandler<DeletePlaylistCommand, Result>
    {
        public async Task<Result> Handle(DeletePlaylistCommand request, CancellationToken ct)
        {
            var playlist = await unitOfWork.Playlists.GetByIdAsync(request.PlaylistId, ct);

            if (!string.IsNullOrEmpty(playlist!.CoverUrl))
                await fileStorageService.DeleteFileAsync(FileType.PlaylistCover, playlist.CoverUrl);

            unitOfWork.Playlists.Delete(playlist, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Ok();
        }
    }
}
