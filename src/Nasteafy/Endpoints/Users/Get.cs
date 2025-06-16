using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Users.Queries.GetById;
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
                return Results.NotFound("User not found");

            return Results.Ok(result.Value);
        }).Produces<GetUserReponse>(StatusCodes.Status200OK)
         .Produces<string>(StatusCodes.Status400BadRequest);
    }
}