using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Artists.Queries.GetAll;
using Nasteafy.Application.Artists.Queries.GetById;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Artists.GetById
{
    public sealed class GetArtistByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/artists/{id:guid}", async ([FromRoute] Guid id, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(new GetArtistByIdQuery(id), ct);

                return response.IsSuccess
                     ? Results.Ok(response.Value)
                     : response.ToApiError();
            })
            .Produces<ArtistDto>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}

