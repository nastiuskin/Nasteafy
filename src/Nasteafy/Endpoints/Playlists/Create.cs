using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Playlists.Commands.Create;

namespace Nasteafy.Endpoints.Playlists
{
    public sealed class CreatePlaylistEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/playlists", async (CreatePlaylistCommand command, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);

                if (result.IsFailed)
                    return Results.BadRequest(result.Errors.First().Message);

                return Results.Ok(result.Value);
            })
            .Produces<Guid>(StatusCodes.Status200OK);
        }
    }
}
