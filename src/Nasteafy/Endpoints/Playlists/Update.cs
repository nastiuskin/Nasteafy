using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Playlists.Commands.Update;
using System.Web.Http;

namespace Nasteafy.Endpoints.Playlists
{
    [Authorize]
    public sealed class UpdatePlaylistEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            //routes.MapPut("api/playlists/{id:guid}", async ([FromForm] string? email, [FromForm] IFormFile? file, ISender sender, CancellationToken ct) =>
            routes.MapPut("api/playlists/{id:guid}", async ([FromRoute] Guid id, [FromForm] UpdatePlaylistRequest request, ISender sender,
                CancellationToken ct) =>
            {
                var command = new UpdatePlaylistCommand(id, request.Title, request.CoverFile);
                var response = await sender.Send(command, ct);

                if (response.IsFailed)
                    return Results.BadRequest(response.Errors.Select(e => e.Message));

                return Results.Ok();
            })
            .Accepts<IFormFile>("multipart/form-data")
            .Produces(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .DisableAntiforgery();
        }
    }
}