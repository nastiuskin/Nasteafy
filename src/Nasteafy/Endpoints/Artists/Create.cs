using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Artists.Commands.Create;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Artists
{
    public sealed class CreatArtistEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/artists", async ([FromForm] CreateArtistCommand command, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);

                if (result.IsFailed)
                    return result.ToApiError();

                return Results.Ok(result.Value);
            })
            .RequireAuthorization("AdminOnly")
            .DisableAntiforgery()
            .Produces<Guid>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}

