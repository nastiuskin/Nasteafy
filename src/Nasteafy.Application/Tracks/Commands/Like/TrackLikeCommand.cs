using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Tracks.Commands.Like;

public record TrackLikeCommand(Guid TrackId, bool Liked) : IRequest<Result>;

public class TrackLikeCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUser) : IRequestHandler<TrackLikeCommand, Result>
{
    public async Task<Result> Handle(TrackLikeCommand request, CancellationToken ct)
    {
        var userId = currentUser.GetUserId();

        var track = await unitOfWork.Tracks.GetByIdWithIncludeAsync(
            request.TrackId,
            ct,
            q => q.Include(t => t.TrackLikes));

        var existingLike = track!.TrackLikes.FirstOrDefault(l => l.UserId == userId);

        if (request.Liked && existingLike is null)
        {
            track.TrackLikes.Add(new TrackLike { UserId = userId, TrackId = request.TrackId });
        }
        else if (!request.Liked && existingLike is not null)
        {
            track.TrackLikes.Remove(existingLike);
        }

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Ok();
    }
}
