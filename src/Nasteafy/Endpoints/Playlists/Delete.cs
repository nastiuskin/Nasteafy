using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Playlists.Commands.Delete;
using Nasteafy.Extensions;
using System.Web.Http;

namespace Nasteafy.Endpoints.Playlists
{
    [Authorize]
    public sealed class DeletePlaylistEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapDelete("api/playlists/{playlistId:guid}", async (Guid playlistId, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new DeletePlaylistCommand(playlistId), ct);

                if (result.IsFailed)
                    return result.ToApiError();

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}