using FluentResults;
using MediatR;
using Nasteafy.Application.Artists.Queries.GetAll;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Playlists.Commands.RemoveFromPlaylist;
using Nasteafy.Domain;

namespace Nasteafy.Application.Artists.Queries.GetByUserId
{
    public record GetArtistByUserIdQuery() : IRequest<Result<ArtistDto>>;

    public class GetArtistByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserProvider currentUserProvider,
        IFileStorageService fileStorageService) : IRequestHandler<GetArtistByUserIdQuery, Result<ArtistDto>>
    {
        public async Task<Result<ArtistDto>> Handle(GetArtistByUserIdQuery request, CancellationToken ct)
        {
            var userId = currentUserProvider.GetUserId();

            if (userId == Guid.Empty)
            {
                return Result.Fail("User not authenticated").Log<GetArtistByIdQueryHandler>();
            }

            var artist = await unitOfWork.Artists.GetByUserIdAsync(userId, ct);

            var avatarUrl = !string.IsNullOrEmpty(artist!.AvatarUrl)
                    ? await fileStorageService.GetFileUrlAsync(FileType.UserAvatar, artist.AvatarUrl)
                    : null;

            var reponse = new ArtistDto(
                artist.Id,
                avatarUrl?.Value,
                artist.Name,
                artist.Biography,
                artist.CreatedByAdmin
            );

            return Result.Ok(reponse);
        }
    }
}

