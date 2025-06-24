using MediatR;
using Microsoft.AspNetCore.Builder;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Users.Queries.GetById;
using Nasteafy.Extensions;
using System.Web.Http;

public sealed class GetUserProfileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routes)
    {
        routes.MapGet("api/users/profile", async (ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetUserProfileQuery(), ct);

            if (result.IsFailed || result.Value is null)
                return result.ToApiError();

            return Results.Ok(result.Value);
        })
        .RequireAuthorization()
        .Produces<GetUserResponse>(StatusCodes.Status200OK)
        .Produces<ApiError>(StatusCodes.Status400BadRequest);
    }
}