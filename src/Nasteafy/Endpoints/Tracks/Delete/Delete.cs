using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Tracks.Commands.Delete;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Tracks.Delete
{
    public sealed class DeleteTrackEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapDelete("api/tracks/{trackId:guid}", async (
                [FromRoute] Guid trackId,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(new DeleteTrackCommand(trackId), ct);

                return result.IsSuccess
                    ? Results.Ok()
                    : result.ToApiError();
            })
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
