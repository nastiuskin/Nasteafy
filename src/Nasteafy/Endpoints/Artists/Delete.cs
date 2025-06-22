using MediatR;
using Microsoft.AspNetCore.Authorization;
using Nasteafy.Abstractions;
using Nasteafy.Application.Artists.Commands.Delete;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Artists
{
    [Authorize(Roles = "Admin")]
    public sealed class DeleteArtistEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapDelete("api/artists/{artistId:guid}", async (Guid artistId, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new DeleteArtistCommand(artistId), ct);

                if (result.IsFailed)
                    return result.ToApiError();

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}

