using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Albums.Commands.Update;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Albums.Update
{
    public sealed class UpdateAlbumEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPut("api/albums/{id:guid}", async (
                [FromRoute] Guid id,
                [FromForm] UpdateAlbumRequest request,
                ISender sender,
                CancellationToken ct) =>
         {
             var command = new UpdateAlbumCommand(id, request.CoverFile, request.ReleaseDate, request.Title);
             var response = await sender.Send(command, ct);

             return response.IsSuccess
                    ? Results.Ok()
                    : response.ToApiError();
         })
         .RequireAuthorization("AdminOnly")
         .DisableAntiforgery()
         .Accepts<UpdateAlbumRequest>("multipart/form-data")
         .Produces(StatusCodes.Status200OK)
         .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
