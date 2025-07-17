using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Playlists.Queries.GetByUserId;
using Nasteafy.Extensions;
using System.Web.Http;

namespace Nasteafy.Endpoints.Playlists.GetAll
{
    public sealed class GetUserPlaylistsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/playlists/paginated-search", async ([FromBody] PagedRequest pagedRequest, ISender sender, CancellationToken ct) =>
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
