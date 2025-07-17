using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Artists.Commands.Delete;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Artists.Delete
{
    public sealed class DeleteArtistEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapDelete("api/artists/{artistId:guid}", async ([FromRoute] Guid artistId, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new DeleteArtistCommand(artistId), ct);

                return result.IsFailed
                    ? result.ToApiError()
                    : Results.NoContent();
            })
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}

