using FluentResults;
using MediatR;
using Nasteafy.Application.Artists.Commands.Delete;
using Nasteafy.Application.Artists.Queries.GetAll;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;

namespace Nasteafy.Application.Artists.Queries.GetById
{
    public record GetArtistByIdQuery(Guid ArtistId) : IRequest<Result<ArtistDto>>;

    public class GetArtistByIdQueryHandler(IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService)
    : IRequestHandler<GetArtistByIdQuery, Result<ArtistDto>>
    {
        public async Task<Result<ArtistDto>> Handle(GetArtistByIdQuery request, CancellationToken ct)
        {
            var artist = await unitOfWork.Artists
                .GetByIdAsync(request.ArtistId, ct);

            if (artist is null)
                return Result.Fail("Artist not found.")
                    .LogIfFailed<GetArtistByIdQueryHandler>(); 

            var avatarUrl = !string.IsNullOrEmpty(artist.AvatarUrl)
                    ? await fileStorageService.GetFileUrlAsync(FileType.UserAvatar, artist.AvatarUrl)
                    : null;


            var dto = new ArtistDto(
                artist.Id,
                avatarUrl?.Value,
                artist.Name,
                artist.CreatedByAdmin
            );

            return Result.Ok(dto);
        }
    }
}
