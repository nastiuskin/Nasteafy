using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Tracks.Commands.AddTrack;
using Nasteafy.Extensions;

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
                    return result.ToApiError();

                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest); 
        }
    }
}