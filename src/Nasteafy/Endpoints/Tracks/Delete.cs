using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Tracks.Commands.Delete;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Tracks
{
    public sealed class DeleteTrackEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapDelete("api/tracks/{trackId:guid}", async (Guid trackId, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new DeleteTrackCommand(trackId), ct);

                if (result.IsFailed)
                    return result.ToApiError();

                return Results.NoContent();
            })
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
