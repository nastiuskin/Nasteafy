using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Tracks.Queries.GetByArtistId;
using Nasteafy.Application.Tracks.Queries.GetById;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Tracks.GetByArtistId
{
    public sealed class GetByArtistIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/artists/{artistId:guid}/tracks/paginated-search", async ([FromRoute] Guid artistId, [FromBody] PagedRequest pagedRequest,
                ISender sender,
                CancellationToken ct) =>
            {
                var response = await sender.Send(new GetTracksByArtistIdQuery(artistId, pagedRequest), ct);

                return response.IsSuccess
                    ? Results.Ok(response.Value)
                    : response.ToApiError();
            })
            .Produces<PagedResult<GetTrackDto>>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}

