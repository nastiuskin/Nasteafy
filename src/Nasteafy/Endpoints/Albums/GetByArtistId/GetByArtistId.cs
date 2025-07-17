using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Albums.Queries.GetByArtistId;
using Nasteafy.Application.Albums.Queries.GetById;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Albums.GetByArtistId
{
    public sealed class GetAlbumByArtistIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/artists/{artistId:guid}/albums/paginated-search", async ([FromRoute] Guid artistId, [FromBody] PagedRequest pagedRequest,
                ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(new GetAlbumsByArtistIdQuery(artistId, pagedRequest), ct);

                return response.IsSuccess
                     ? Results.Ok(response.Value)
                     : response.ToApiError();
            })
            .Produces<PagedResult<AlbumDto>>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}

