using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Playlists.Commands.Update;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Playlists.Update
{
    public sealed class UpdatePlaylistEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPut("api/playlists/{id:guid}", async ([FromRoute] Guid id,
                [FromForm] UpdatePlaylistRequest request, ISender sender, CancellationToken ct) =>
            {
                var command = new UpdatePlaylistCommand(
                    PlaylistId: id,
                    Title: request.Title,
                    CoverFile: request.CoverFile);

                var response = await sender.Send(command, ct);

                return response.IsSuccess
                    ? Results.Ok()
                    : response.ToApiError();
            })
            .RequireAuthorization()
            .Accepts<UpdatePlaylistRequest>("multipart/form-data")
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest)
            .DisableAntiforgery();
        }
    }
}