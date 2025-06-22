using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Tracks.Queries.GetById;
using Nasteafy.Application.Tracks.Queries.GetByPlaylistId;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Tracks
{
    public sealed class GetByPlaylistIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/playlists/{playlistId:guid}/tracks", async (ISender sender, [FromRoute] Guid playlistId, [AsParameters] PagedRequest pagedRequest, CancellationToken ct) =>
            {
                var response = await sender.Send(new GetTracksByPlaylistIdQuery(playlistId, pagedRequest), ct);

                if (response.IsFailed)
                    return response.ToApiError();

                return Results.Ok(response.Value);
            })
            .Produces<PagedResult<GetTrackDto>>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
