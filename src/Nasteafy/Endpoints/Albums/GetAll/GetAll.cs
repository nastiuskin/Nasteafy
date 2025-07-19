using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Albums.Queries.GetAll;
using Nasteafy.Application.Albums.Queries.GetById;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Albums.GetAll
{
    public class GetAllAlbumsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/albums/paginated-search", async ([FromBody] PagedRequest pagedRequest, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(new GetAllAlbumsQuery(pagedRequest), ct);

                return response.IsSuccess
                    ? Results.Ok(response.Value)
                    : response.ToApiError();
            })
            .Produces<PagedResult<AlbumDto>>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
