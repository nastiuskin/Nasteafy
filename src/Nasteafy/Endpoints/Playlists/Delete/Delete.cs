using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Playlists.Commands.Delete;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Playlists.Delete
{
    public sealed class DeletePlaylistEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapDelete("api/playlists/{playlistId:guid}", async ([FromRoute] Guid playlistId, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new DeletePlaylistCommand(playlistId), ct);

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