using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Albums.Commands.Create;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Albums
{
    public sealed class CreatAlbumEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/albums", async ([FromForm] CreateAlbumCommand command, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);

                if (result.IsFailed)
                    return result.ToApiError();

                return Results.Ok(result.Value);
            })
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin, Artist" })
            .Accepts<IFormFile>("multipart/form-data")
            .DisableAntiforgery()
            .Produces<Guid>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
