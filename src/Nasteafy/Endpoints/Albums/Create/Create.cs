using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Albums.Commands.Create;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Albums.Create
{
    public sealed class CreatAlbumEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/albums", async ([FromForm] CreateAlbumRequest request,ISender sender,CancellationToken ct) =>
            {
                var command = new CreateAlbumCommand(request.Title, request.CoverFile, request.ReleaseDate, request.Artists);
                var result = await sender.Send(command, ct);

                return result.IsFailed
                    ? result.ToApiError()
                    : Results.Created();
            })
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin, Artist" })
            .Accepts<CreateAlbumRequest>("multipart/form-data")
            .DisableAntiforgery()
            .Produces(StatusCodes.Status201Created)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
