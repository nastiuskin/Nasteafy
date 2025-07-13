using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Albums.Queries.GetById;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Albums.GetById
{
    public sealed class GetByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/albums/{Id:guid}", async ([FromRoute] Guid Id, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(new GetAlbumByIdQuery(Id), ct);

                return response.IsSuccess
                    ? Results.Ok(response.Value)
                    : response.ToApiError();
            })
            .Produces<AlbumDto>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
