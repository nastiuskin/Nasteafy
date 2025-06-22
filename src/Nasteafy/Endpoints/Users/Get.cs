using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Users.Queries.GetById;
using Nasteafy.Extensions;
using System.Web.Http;

public sealed class GetUserProfileEndpoint : IEndpoint
{
    [Authorize]
    public void MapEndpoint(IEndpointRouteBuilder routes)
    {
        routes.MapGet("api/users/profile", async (ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetUserProfileQuery(), ct);

            if (result.IsFailed || result.Value is null)
                return result.ToApiError();

            return Results.Ok(result.Value);
        })
        .Produces<GetUserResponse>(StatusCodes.Status200OK)
        .Produces<ApiError>(StatusCodes.Status400BadRequest);
    }
}