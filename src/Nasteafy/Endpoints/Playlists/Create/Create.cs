using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Playlists.Commands.Create;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Playlists.Create
{
    public sealed class CreatePlaylistEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/playlists", async ([FromForm] CreatePlaylistRequest request, ISender sender, CancellationToken ct) =>
            {
                var command = new CreatePlaylistCommand(request.Title, request.PlaylistCover);
                var result = await sender.Send(command, ct);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(result.ToApiError());
            })
            .RequireAuthorization()
            .Accepts<CreatePlaylistRequest>("multipart/form-data")
            .DisableAntiforgery()
            .Produces<Guid>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
