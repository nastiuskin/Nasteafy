using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Playlists.Commands.AddTrackToPlaylist;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Playlists
{
    public sealed class AddTrackToPlaylistEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/playlists/{playlistId:guid}/tracks", async ([FromRoute] Guid playlistId,
                [FromBody] AddTrackToPlaylistCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
                command.PlaylistId = playlistId;
                var result = await sender.Send(command, ct);

                if (result.IsFailed)
                    return result.ToApiError();

                return Results.Ok();
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
