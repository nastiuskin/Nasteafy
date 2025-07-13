using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Playlists.Queries.GetById;
using Nasteafy.Application.Playlists.Queries.GetByUserId;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Playlists.GetById
{
    public sealed class GetPlaylistByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/playlists/{Id:guid}", async ([FromRoute] Guid Id, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(new GetPlaylistByIdQuery(Id), ct);

                return response.IsSuccess
                    ? Results.Ok(response.Value)
                    : Results.BadRequest(response.ToApiError());
            })
            .RequireAuthorization()
            .Produces<UserPlaylistDto>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
