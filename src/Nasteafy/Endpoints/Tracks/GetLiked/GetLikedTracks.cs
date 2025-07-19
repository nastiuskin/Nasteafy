using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Tracks.Queries.GetById;
using Nasteafy.Application.Tracks.Queries.GetLiked;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Tracks.Like
{
    public sealed class GetLikedTracksEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/tracks/liked/paginated-search", async ([FromBody] PagedRequest pagedRequest, ISender sender,
                CancellationToken ct) =>
            {
                var response = await sender.Send(new GetLikedTracksQuery(pagedRequest), ct);

                return response.IsSuccess
                    ? Results.Ok(response.Value)
                    : response.ToApiError();
            })
            .RequireAuthorization()
            .Produces<PagedResult<GetTrackDto>>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
