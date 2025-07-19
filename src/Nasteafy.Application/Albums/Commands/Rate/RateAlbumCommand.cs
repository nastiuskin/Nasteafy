using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Playlists.Commands.RemoveFromPlaylist;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Albums.Commands.Rate
{
    public record RateAlbumCommand(Guid AlbumId, int Rating) : IRequest<Result>;

    public class RateAlbumCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserProvider userProvider) : IRequestHandler<RateAlbumCommand, Result>
    {
        public async Task<Result> Handle(RateAlbumCommand request, CancellationToken cancellationToken)
        {
            var userId = userProvider.GetUserId();

            if (userId == Guid.Empty)
            {
                return Result.Fail("User not authenticated").Log<RemoveTrackFromPlaylistCommandHandler>();
            }

            var album = await unitOfWork.Albums.GetByIdWithIncludeAsync(
                request.AlbumId,
                cancellationToken,
                q => q.Include(x => x.Ratings));

            if(album!.Ratings.Any(r => r.UserId == userId))
            {
                var existingRating = album.Ratings.First(r => r.UserId == userId);
                existingRating.Rating = request.Rating; 
            }
            else
            {
                var rating = new AlbumRating
                {
                    AlbumId = request.AlbumId,
                    UserId = userId,
                    Rating = request.Rating
                };

                album!.Ratings.Add(rating);
            }
              
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
