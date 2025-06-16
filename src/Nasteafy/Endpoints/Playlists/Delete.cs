using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Playlists.Commands.Delete;

namespace Nasteafy.Endpoints.Playlists
{
    public sealed class DeletePlaylistEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapDelete("api/playlists/{playlistId:guid}", async (Guid playlistId, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new DeletePlaylistCommand(playlistId), ct);

                if (result.IsFailed)
                    return Results.BadRequest(result.Errors.First().Message);

                return Results.NoContent();
            });
        }
    }
}