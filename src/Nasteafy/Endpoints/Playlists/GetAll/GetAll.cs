using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Playlists.Queries.GetByUserId;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Playlists.GetAll
{
    public sealed class GetUserPlaylistsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/playlists", async (ISender sender, [AsParameters] PagedRequest pagedRequest, CancellationToken ct) =>
            {
                var result = await sender.Send(new GetUserPlaylistsQuery(pagedRequest), ct);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(result.ToApiError());
            })
            .RequireAuthorization()
            .Produces<PagedResult<UserPlaylistDto>>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
