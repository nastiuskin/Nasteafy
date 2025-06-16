using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Users.Queries.GetPlaylists;

namespace Nasteafy.Endpoints.Playlists
{
    public sealed class GetUserPlaylistsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/playlists", async (ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new GetUserPlaylistsQuery(), ct);

                if (result.IsFailed)
                    return Results.BadRequest(result.Errors.First().Message);

                return Results.Ok(result.Value);
            })
            .Produces<GetUserPlaylistsResponse>(StatusCodes.Status200OK)
            .WithTags("Playlists");
        }
    }
}
