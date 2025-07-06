using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Playlists.Commands.Update;
using Nasteafy.Extensions;
using System.Web.Http;

namespace Nasteafy.Endpoints.Playlists
{
    public sealed class UpdatePlaylistEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPut("api/playlists/{id:guid}", async ([FromRoute] Guid id, [FromForm] UpdatePlaylistCommand request, ISender sender,
                CancellationToken ct) =>
            {
                request.PlaylistId = id;
                var response = await sender.Send(request, ct);

                if (response.IsFailed)
                    return response.ToApiError();

                return Results.Ok();
            })
            .RequireAuthorization()
            .Accepts<IFormFile>("multipart/form-data")
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest)
            .DisableAntiforgery();
        }
    }
}