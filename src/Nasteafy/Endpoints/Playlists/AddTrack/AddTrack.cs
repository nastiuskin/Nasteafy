using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Playlists.Commands.AddTrackToPlaylist;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Playlists.AddTrack
{
    public sealed class AddTrackToPlaylistEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/playlists/{playlistId:guid}/tracks/{trackId:guid}", async ([FromRoute] Guid playlistId, [FromRoute] Guid trackId,
                ISender sender,
                CancellationToken ct) =>
            {
                var command = new AddTrackToPlaylistCommand(playlistId, trackId);
                var result = await sender.Send(command, ct);

                return result.IsSuccess
                    ? Results.Ok()
                    : result.ToApiError();
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
