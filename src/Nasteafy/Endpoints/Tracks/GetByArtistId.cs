using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Tracks.Queries.GetByArtistId;
using Nasteafy.Application.Tracks.Queries.GetById;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Tracks
{
    public sealed class GetByArtistIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/artists/{artistId:guid}/tracks", async (ISender sender, [FromRoute] Guid artistId, [AsParameters] PagedRequest pagedRequest, CancellationToken ct) =>
            {
                var response = await sender.Send(new GetTracksByArtistIdQuery(artistId, pagedRequest), ct);

                if (response.IsFailed)
                    return response.ToApiError();

                return Results.Ok(response.Value);
            })
            .Produces<PagedResult<GetTrackDto>>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}

