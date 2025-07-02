using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Albums.Commands.Update;
using Nasteafy.Application.Artists.Commands.Update;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Albums
{
    public sealed class UpdateAlbumEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPut("api/albums/{id:guid}", async ([FromRoute] Guid id, [FromForm] UpdateAlbumCommand command,
               ISender sender, CancellationToken ct) =>
         {
             command.AlbumId = id;
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
