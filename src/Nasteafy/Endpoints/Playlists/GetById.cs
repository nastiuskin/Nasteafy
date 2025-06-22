using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Playlists.Queries.GetById;
using Nasteafy.Application.Playlists.Queries.GetByUserId;
using Nasteafy.Extensions;
using System.Web.Http;

namespace Nasteafy.Endpoints.Playlists
{
    [Authorize]
    public sealed class GetPlaylistByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/playlists/{Id:guid}", async ([FromRoute] Guid Id, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(new GetPlaylistByIdQuery(Id), ct);

                if (response.IsFailed)
                    return response.ToApiError();

                return Results.Ok(response.Value);
            })
            .Produces<UserPlaylistDto>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
