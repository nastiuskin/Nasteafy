using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Models;
using Nasteafy.Domain;

namespace Nasteafy.Application.Playlists.Queries.GetByUserId
{
    public record GetUserPlaylistsQuery(PagedRequest PagedRequest) : IRequest<Result<PagedResult<UserPlaylistDto>>>;

    public class GetUserPlaylistsQueryHandler(
     IUnitOfWork unitOfWork,
     IFileStorageService fileStorageService,
     IUserIdProvider userProvider)
     : IRequestHandler<GetUserPlaylistsQuery, Result<PagedResult<UserPlaylistDto>>>
    {
        public async Task<Result<PagedResult<UserPlaylistDto>>> Handle(GetUserPlaylistsQuery query, CancellationToken ct)
        {
            var userId = userProvider.GetUserId();
            if (userId == null  || userId == Guid.Empty)
                return Result.Fail("UserId not found");

            var playlists = await unitOfWork.Playlists.GetByUserIdAsync(userId.Value, query.PagedRequest, ct);

            var playlistDtos = new List<UserPlaylistDto>();

            foreach (var p in playlists.Items)
            {
                var coverUrl = !string.IsNullOrEmpty(p.CoverUrl)
                    ? await fileStorageService.GetFileUrlAsync(FileType.PlaylistCover, p.CoverUrl)
                    : null;

                playlistDtos.Add(new UserPlaylistDto(
                    p.Id,
                    p.Title,
                    coverUrl,
                    p.PlaylistTracks.Count));
            }

            var result = new PagedResult<UserPlaylistDto>
            {
                Items = playlistDtos,
                TotalItems = playlists.TotalItems,
                PageNumber = playlists.PageNumber,
                PageSize = playlists.PageSize
            };

            return Result.Ok(result);
        }
    }
}

