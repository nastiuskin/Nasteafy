using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Tracks.Commands.Create;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Tracks
{
    [Authorize]
    public sealed class AddTrackEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/tracks", async ([FromForm] CreateTrackCommand command, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(command, ct);

                if (response.IsFailed)
                    return response.ToApiError();

                return Results.Ok();
            })
            //.Accepts<IFormFile>("multipart/form-data")
            .DisableAntiforgery()
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}

