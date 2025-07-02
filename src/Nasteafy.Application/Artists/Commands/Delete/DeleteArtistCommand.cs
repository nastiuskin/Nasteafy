using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Artists.Commands.Delete
{
    public record DeleteArtistCommand(Guid ArtistId) : IRequest<Result>;

    public class DeleteArtistCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteArtistCommand, Result>
    {
        public async Task<Result> Handle(DeleteArtistCommand request, CancellationToken ct)
        {
            var artist = await unitOfWork.Artists
                .GetByIdWithIncludeAsync(
                    request.ArtistId,
                    ct,
                    x => x.Include(x => x.ArtistTracks)
                        .Include(x => x.AlbumArtists)                   
                );

            if (artist is null)
                return Result.Fail("Artist not found.").Log<DeleteArtistCommandHandler>();

            bool hasTracks = artist.ArtistTracks.Any();
            bool hasAlbums = artist.AlbumArtists.Any();

            if (hasTracks || hasAlbums)
                return Result.Fail("Cannot delete artist with associated tracks or albums.").Log<DeleteArtistCommandHandler>();

            await unitOfWork.Artists.DeleteAsync(artist, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Ok();
        }
    }
}