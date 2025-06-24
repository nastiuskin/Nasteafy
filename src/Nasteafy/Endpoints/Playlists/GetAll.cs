using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Playlists.Queries.GetByUserId;
using Nasteafy.Extensions;
using System.Web.Http;

namespace Nasteafy.Endpoints.Playlists
{
    public sealed class GetUserPlaylistsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/playlists", async (ISender sender, [AsParameters] PagedRequest pagedRequest, CancellationToken ct) =>
            {
                var result = await sender.Send(new GetUserPlaylistsQuery(pagedRequest), ct);

                if (result.IsFailed)
                    return result.ToApiError();

                return Results.Ok(result.Value);
            })
            .RequireAuthorization()
            .Produces<PagedResult<UserPlaylistDto>>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
