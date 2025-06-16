using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Abstractions.Auth;
using Nasteafy.Application.Abstractions.Data;
using Nasteafy.Application.Playlists.Queries.GetById;
using Nasteafy.Domain;

namespace Nasteafy.Application.Users.Queries.GetPlaylists
{
    public record GetUserPlaylistsQuery : IRequest<Result<GetUserPlaylistsResponse>>;

    public class GetUserPlaylistsQueryHandler(
     IUnitOfWork unitOfWork,
     IFileStorageService fileStorageService,
     IUserIdProvider userProvider)
     : IRequestHandler<GetUserPlaylistsQuery, Result<GetUserPlaylistsResponse>>
    {
        public async Task<Result<GetUserPlaylistsResponse>> Handle(GetUserPlaylistsQuery request, CancellationToken ct)
        {
            var userId = userProvider.GetUserId();
            if (userId == null  || userId == Guid.Empty)
                return Result.Fail("UserId not found");

            var query = unitOfWork.Playlists.GetByUserIdWithTracks(userId.Value, ct);

            var rawPlaylists = await query
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.CoverUrl,
                    Tracks = p.PlaylistTracks.Select(pt => new
                    {
                        pt.Track.Id,
                        pt.Track.Title,
                        pt.Track.Duration,
                        pt.Track.CoverUrl,
                        Artists = pt.Track.ArtistTracks.Select(at => at.Artist.Name).ToList()
                    }).ToList()
                })
                .ToListAsync(ct);
            var playlistDtos = new List<PlaylistDetailsDto>();

            foreach (var p in rawPlaylists)
            {
                string? coverUrl = null;
                if (!string.IsNullOrEmpty(p.CoverUrl))
                {
                    coverUrl = await fileStorageService.GetFileUrlAsync(FileType.PlaylistCover, p.CoverUrl);
                }

                var trackDtos = p.Tracks.Select(t => new PlaylistTrackDto(
                    t.Id,
                    t.Title,
                    string.Join(", ", t.Artists),
                    t.Duration,
                    t.CoverUrl
                )).ToList();

                playlistDtos.Add(new PlaylistDetailsDto(
                    p.Id,
                    p.Title,
                    coverUrl,
                    trackDtos.Count,
                    trackDtos
                ));
            }

            return Result.Ok(new GetUserPlaylistsResponse { Playlists = playlistDtos });
        }
    }
}
