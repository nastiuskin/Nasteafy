using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Tracks.Commands.Like;

namespace Nasteafy.Endpoints.Tracks.Like
{
    public class TrackLikeEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPut("api/tracks/{id:guid}/like", async ([FromRoute] Guid id, [FromQuery] bool liked, ISender sender,
                CancellationToken ct) =>
            {
                var command = new TrackLikeCommand(id, liked);
                var result = await sender.Send(new TrackLikeCommand(id, liked), ct);

                return result.IsSuccess
                    ? Results.Ok()
                    : Results.BadRequest(result.Errors);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
