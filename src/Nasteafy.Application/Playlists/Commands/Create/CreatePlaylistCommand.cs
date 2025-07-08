using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Helpers;
using Nasteafy.Domain;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Playlists.Commands.Create
{
    public class CreatePlaylistCommand : IRequest<Result<Guid>>, ITransactionalCommand
    {
        public required string Title { get; set; }
        public IFormFile? PlaylistCover { get; set; }
    }

    public class CreatePlaylistCommandHandler(
        IUnitOfWork _unitOfWork,
        ICurrentUserProvider _userProvider,
        IFileStorageService fileStorageService) : IRequestHandler<CreatePlaylistCommand, Result<Guid>>
    {

        public async Task<Result<Guid>> Handle(CreatePlaylistCommand request, CancellationToken cancellationToken)
        {
            var userId = _userProvider.GetUserId();

            if (userId == null || userId == Guid.Empty)
            {
                return Result.Fail("UserId not found").Log<CreatePlaylistCommandHandler>();
            }               

            var playlist = new Playlist
            {
                Title = request.Title,
                UserId = userId,
            };

            if (request.PlaylistCover != null && request?.PlaylistCover?.Length > 0)
            {
                await using var stream = request.PlaylistCover.OpenReadStream();

                var result = await fileStorageService.UploadFileAsync(
                    stream,
                    request.PlaylistCover.FileName,
                    request.PlaylistCover.ContentType,
                    FileType.PlaylistCover);

                if (result.IsSuccess)
                {
                    playlist.CoverUrl = result.Value;
                }                    
            }

            await _unitOfWork.Playlists.AddAsync(playlist, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Ok(playlist.Id);
        }
    }
}
