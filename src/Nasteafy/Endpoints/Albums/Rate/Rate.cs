using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Albums.Commands.Rate;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Albums.Rate
{
    public class RateAlbumEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/albums/{albumId}/rate", async ([FromRoute] Guid albumId, RateAlbumRequest request, ISender sender, CancellationToken ct) =>
            {
                var command = new RateAlbumCommand(albumId, request.Rating);
                var result = await sender.Send(command, ct);

                return result.IsFailed
                    ? result.ToApiError()
                    : Results.Ok();
            })
            .RequireAuthorization()
            .Accepts<RateAlbumRequest>("application/json")
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
