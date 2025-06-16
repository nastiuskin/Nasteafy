using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Abstractions.Data;
using Nasteafy.Domain;

namespace Nasteafy.Application.Playlists.Queries.GetById
{
    public record GetPlaylistByIdQuery(Guid PlaylistId) : IRequest<Result<PlaylistDetailsDto>>;

    public class GetPlaylistByIdQueryHandler(IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService)
       : IRequestHandler<GetPlaylistByIdQuery, Result<PlaylistDetailsDto>>
    {
        public async Task<Result<PlaylistDetailsDto>> Handle(GetPlaylistByIdQuery req, CancellationToken ct)
        {
            var query = unitOfWork.Playlists.GetByIdWithTracks(req.PlaylistId, ct);

            var rawPlaylist = await query
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
                .FirstOrDefaultAsync(ct);

            if (rawPlaylist is null)
                return Result.Fail("Playlist not found");

            var trackDtos = rawPlaylist.Tracks.Select(t => new PlaylistTrackDto(
                t.Id,
                t.Title,
                string.Join(", ", t.Artists),
                t.Duration,
                t.CoverUrl
            )).ToList();

            string? coverUrl = null;
            if (!string.IsNullOrEmpty(rawPlaylist.CoverUrl))
            {
                coverUrl = await fileStorageService.GetFileUrlAsync(FileType.PlaylistCover, rawPlaylist.CoverUrl);
            }

            var playlistDto = new PlaylistDetailsDto(
                rawPlaylist.Id,
                rawPlaylist.Title,
                coverUrl,
                trackDtos.Count,
                trackDtos
            );

            return Result.Ok(playlistDto);
        }
    }
}
