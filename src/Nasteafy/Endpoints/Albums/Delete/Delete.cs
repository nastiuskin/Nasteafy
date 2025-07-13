using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Albums.Commands.Delete;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Albums.Delete
{
    public sealed class DeleteAlbumEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapDelete("api/albums/{albumId:guid}", async (Guid albumId, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new DeleteAlbumCommand(albumId), ct);

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
