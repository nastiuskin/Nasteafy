using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;

namespace Nasteafy.Application.Tracks.Commands.Delete
{
    public record DeleteTrackCommand(Guid TrackId) : IRequest<Result>;

    public class DeleteTrackCommandHandler(IUnitOfWork unitOfWork, IFileStorageService fileStorageService)
       : IRequestHandler<DeleteTrackCommand, Result>
    {
        public async Task<Result> Handle(DeleteTrackCommand request, CancellationToken ct)
        {
            var track = await unitOfWork
                .Tracks
                .GetByIdAsync(request.TrackId, ct);

            if (track == null)
                return Result.Fail("Track not found");

            if (!string.IsNullOrEmpty(track.FilePath))
                await fileStorageService.DeleteFileAsync(FileType.Audio, track.FilePath);

            await unitOfWork.Tracks.DeleteAsync(request.TrackId, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Ok();
        }
    }
}


