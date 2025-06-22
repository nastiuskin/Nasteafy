using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Albums.Queries.GetById;
using Nasteafy.Application.Artists.Queries.GetAll;
using Nasteafy.Application.Artists.Queries.GetById;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Playlists.Queries.GetByUserId;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Artists
{
    public sealed class GetArtistByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/artists/{Id:guid}", async ([FromRoute] Guid Id, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(new GetArtistByIdQuery(Id), ct);

                if (response.IsFailed)
                    return response.ToApiError();

                return Results.Ok(response.Value);
            })
            .Produces<ArtistDto>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}

