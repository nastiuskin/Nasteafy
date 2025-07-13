using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Artists.Queries;
using Nasteafy.Application.Artists.Queries.GetAll;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;
using System.Web.Http;

namespace Nasteafy.Endpoints.Artists
{
    public sealed class GetAllArtistsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/artists/paginated-search", async (
                [FromBody] PagedRequest pagedRequest,
                ISender sender,
                CancellationToken ct) =>
            {
                var response = await sender.Send(new GetAllArtistsQuery(pagedRequest), ct);

                return response.IsSuccess
                     ? Results.Ok(response.Value)
                     : response.ToApiError();
            })
            .Produces<PagedResult<ArtistDto>>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}

