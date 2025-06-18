using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Playlists.Commands.Create;
using Nasteafy.Extensions;

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
                    return result.ToApiError();

                return Results.Ok(result.Value);
            })
            .Produces<Guid>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
