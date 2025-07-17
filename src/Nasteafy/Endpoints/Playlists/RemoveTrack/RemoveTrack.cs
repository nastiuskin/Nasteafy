using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Playlists.Commands.AddTrackToPlaylist;
using Nasteafy.Application.Playlists.Commands.RemoveFromPlaylist;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Playlists.RemoveTrack
{
    public sealed class RemoveTrackFromPlaylistEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapDelete("api/playlists/{playlistId:guid}/tracks/{trackId:guid}", async ([FromRoute] Guid playlistId, [FromRoute] Guid trackId,
                ISender sender,
                CancellationToken ct) =>
            {
                var command = new RemoveTrackFromPlaylistCommand(playlistId, trackId);
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
