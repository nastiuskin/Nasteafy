using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Tracks.Queries.GetByAlbumId;
using Nasteafy.Application.Tracks.Queries.GetById;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Tracks.GetByAlbumId
{
    public sealed class GetByAlbumIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/albums/{albumId:guid}/tracks", async (                 
                [FromRoute] Guid albumId,
                [AsParameters] PagedRequest pagedRequest,
                ISender sender, 
                CancellationToken ct) =>
            {
                var response = await sender.Send(new GetTracksByAlbumIdQuery(albumId, pagedRequest), ct);

                return response.IsSuccess
                    ? Results.Ok(response.Value)
                    : response.ToApiError();
            })
            .Produces<PagedResult<GetTrackDto>>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}

