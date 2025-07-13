using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Artists.Commands.Update;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Artists.Update
{
    public sealed class UpdateArtistEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPut("api/artists/{id:guid}", async ([FromRoute] Guid id,
                [FromForm] UpdateArtistRequest request, ISender sender, CancellationToken ct) =>
            {
                var command = new UpdateArtistCommand(id, request.Name, request.AvatarFile);
                var response = await sender.Send(command, ct);

                return response.IsSuccess
                   ? Results.Ok()
                   : response.ToApiError();
            })
            .RequireAuthorization("AdminOnly")
            .DisableAntiforgery()
            .Accepts<UpdateArtistRequest>("multipart/form-data")
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}

