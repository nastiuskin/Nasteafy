using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Artists.Queries.GetAll;
using Nasteafy.Application.Artists.Queries.GetByUserId;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Artists.GetByUserId
{
    public class GetArtistByUserIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/artists/user/", async (ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(new GetArtistByUserIdQuery(), ct);
                return response.IsSuccess
                     ? Results.Ok(response.Value)
                     : response.ToApiError();
            })
            .Produces<ArtistDto>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}