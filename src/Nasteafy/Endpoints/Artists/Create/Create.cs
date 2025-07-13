using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Artists.Commands.Create;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Artists.Create
{
    public sealed class CreatArtistEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/artists", async ([FromForm] CreateArtistRequest request, ISender sender, CancellationToken ct) =>
            {
                var command = new CreateArtistCommand(request.Name, request.ArtistPhoto);
                var result = await sender.Send(command, ct);

                return result.IsSuccess
                    ? Results.Created()
                    : result.ToApiError();
            })
            .RequireAuthorization("AdminOnly")
            .Accepts<CreateArtistRequest>("multipart/form-data")
            .DisableAntiforgery()
            .Produces(StatusCodes.Status201Created)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}

