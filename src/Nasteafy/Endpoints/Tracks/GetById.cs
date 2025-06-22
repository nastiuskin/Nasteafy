using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Tracks.Queries.GetById;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Tracks
{
    public sealed class GetTrackByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/tracks/{Id:guid}", async ([FromRoute] Guid Id, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(new GetTrackByIdQuery(Id), ct);

                if (response.IsFailed)
                    return response.ToApiError();

                return Results.Ok(response.Value);
            })
            .Produces<GetTrackDto>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}

