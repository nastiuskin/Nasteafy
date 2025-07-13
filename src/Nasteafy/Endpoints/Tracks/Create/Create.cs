using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Tracks.Commands.Create;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Tracks.Create
{   
    public sealed class CreateTrackEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/tracks", async ([FromForm] CreateTrackRequest request, ISender sender, CancellationToken ct) =>
            {
                var command = new CreateTrackCommand(request.File, request.Title, request.Duration, request.AlbumId, request.Artists);
                var response = await sender.Send(command, ct);

               return response.IsSuccess
                    ? Results.Ok(response.Value)
                    : response.ToApiError();
            })
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,Artist" })
            .Accepts<CreateTrackRequest>("multipart/form-data")
            .DisableAntiforgery()
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}

