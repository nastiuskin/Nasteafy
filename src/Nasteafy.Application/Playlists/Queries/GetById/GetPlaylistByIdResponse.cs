using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasteafy.Application.Playlists.Queries.GetById
{
    public record PlaylistTrackDto(
         Guid Id,
         string Title,
         string Artist,
         TimeSpan Duration,
         string? CoverUrl);

    public record PlaylistDetailsDto(
        Guid Id,
        string Title,
        string? CoverUrl,
        int TracksCount,
        List<PlaylistTrackDto> Tracks);
}
