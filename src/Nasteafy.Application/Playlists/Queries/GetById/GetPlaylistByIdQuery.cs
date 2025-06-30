using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Playlists.Queries.GetByUserId;
using Nasteafy.Domain;

namespace Nasteafy.Application.Playlists.Queries.GetById
{
    public record GetPlaylistByIdQuery(Guid PlaylistId) : IRequest<Result<UserPlaylistDto>>;

    public class GetPlaylistByIdQueryHandler(IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService)
       : IRequestHandler<GetPlaylistByIdQuery, Result<UserPlaylistDto>>
    {
        public async Task<Result<UserPlaylistDto>> Handle(GetPlaylistByIdQuery req, CancellationToken ct)
        {
            var playlist = await unitOfWork.Playlists.GetByIdWithTracks(req.PlaylistId, ct);

            string? coverUrl = null;
            if (!string.IsNullOrEmpty(playlist!.CoverUrl))
            {
                var result = await fileStorageService.GetFileUrlAsync(FileType.PlaylistCover, playlist.CoverUrl);
                coverUrl = result.Value;
            }

            var playlistDto = new UserPlaylistDto(
                playlist.Id,
                playlist.Title,
                coverUrl,
                playlist.PlaylistTracks.Count);

            return Result.Ok(playlistDto);
        }
    }
}
