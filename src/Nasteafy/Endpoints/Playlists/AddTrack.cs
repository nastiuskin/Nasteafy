using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Playlists.Commands.AddTrack;

namespace Nasteafy.Endpoints.Playlists
{
    public sealed class AddTrackToPlaylistEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/playlists/{playlistId:guid}/tracks/{trackId:guid}", async (
                Guid playlistId,
                Guid trackId,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(new AddTrackToPlaylistCommand(playlistId, trackId), ct);

                if (result.IsFailed)
                    return Results.BadRequest(result.Errors.First().Message);

                return Results.NoContent();
            });
        }
    }
}