using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Helpers;
using Nasteafy.Domain;

namespace Nasteafy.Application.Tracks.Commands.Delete
{
    public record DeleteTrackCommand(Guid TrackId) : IRequest<Result>, ITransactionalCommand;

    public class DeleteTrackCommandHandler(IUnitOfWork unitOfWork, IFileStorageService fileStorageService)
       : IRequestHandler<DeleteTrackCommand, Result>
    {
        public async Task<Result> Handle(DeleteTrackCommand request, CancellationToken ct)
        {
            var track = await unitOfWork.Tracks.GetByIdAsync(request.TrackId, ct);

            if (!string.IsNullOrEmpty(track!.FilePath))
            {
                await fileStorageService.DeleteFileAsync(FileType.Audio, track.FilePath);
            }               

            unitOfWork.Tracks.Delete(track, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Ok();
        }
    }
}


