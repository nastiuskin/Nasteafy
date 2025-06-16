using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Playlists.Queries.GetById;

namespace Nasteafy.Endpoints.Playlists
{
    public sealed class GetPlaylistByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/playlists/{Id:guid}", async ([FromRoute] Guid Id, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(new GetPlaylistByIdQuery(Id), ct);

                if (response.IsFailed)
                    return Results.BadRequest(response.Errors.Select(e => e.Message));

                return Results.Ok(response.Value);
            })
            .Produces<PlaylistDetailsDto>(StatusCodes.Status200OK);
        }
    }
}
