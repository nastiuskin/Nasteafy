using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Artists.Queries;
using Nasteafy.Application.Artists.Queries.GetAll;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Artists
{
    public sealed class GetAllArtistsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/artists", async (ISender sender, [AsParameters] PagedRequest pagedRequest, CancellationToken ct) =>
            {
                var result = await sender.Send(new GetAllArtistsQuery(pagedRequest), ct);

                if (result.IsFailed)
                    return result.ToApiError();

                return Results.Ok(result.Value);
            })
            .Produces<PagedResult<ArtistDto>>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}

