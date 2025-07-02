using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Artists.Commands.Update;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Users.Commands.Update;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Artists
{
    public sealed class UpdateArtistEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPut("api/artists/{id:guid}", async ([FromRoute] Guid id, [FromForm] UpdateArtistCommand command, ISender sender, CancellationToken ct) =>
            {
                command.ArtistId = id;
                var response = await sender.Send(command, ct);

                if (response.IsFailed)
                    return response.ToApiError();

                return Results.Ok();
            })
            .RequireAuthorization("AdminOnly")
            .DisableAntiforgery()
            .Accepts<IFormFile>("multipart/form-data")
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}

