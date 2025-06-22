using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Artists.Commands.Create
{
    public record CreateArtistCommand(string Name, IFormFile? ArtistPhoto) : IRequest<Result<Guid>>;

    public class CreateArtistCommandHandler(IUnitOfWork unitOfWork, 
        IFileStorageService fileStorageService)
    : IRequestHandler<CreateArtistCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateArtistCommand request, CancellationToken ct)
        {
            var artist = new Artist
            {
                Name = request.Name.Trim(),
                AlbumArtists = [],
                ArtistTracks = [],
                CreatedByAdmin = true
            };

            if (request.ArtistPhoto != null || request?.ArtistPhoto?.Length > 0)
            {
                await using var stream = request.ArtistPhoto.OpenReadStream();

                var result = await fileStorageService.UploadFileAsync(
                    stream,
                    request.ArtistPhoto.FileName,
                    request.ArtistPhoto.ContentType,
                    FileType.UserAvatar);

                artist.AvatarUrl = result.Value;
            }

            await unitOfWork.Artists.AddAsync(artist, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Ok(artist.Id);
        }
    }
}
