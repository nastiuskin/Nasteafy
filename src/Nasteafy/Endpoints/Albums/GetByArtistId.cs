using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Albums.Queries.GetByArtistId;
using Nasteafy.Application.Albums.Queries.GetById;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Playlists.Queries.GetByUserId;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Albums
{
    public sealed class GetByArtistIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/artists/{artistId:guid}/albums", async ([FromRoute] Guid artistId,
                [AsParameters] PagedRequest pagedRequest,
                ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(new GetByArtistIdQuery(artistId, pagedRequest), ct);

                if (response.IsFailed)
                    return response.ToApiError();

                return Results.Ok(response.Value);
            })
            .Produces<PagedResult<AlbumDto>>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}

