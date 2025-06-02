using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Users.Commands.Login;

namespace Nasteafy.Endpoints.Users
{
    public sealed class LoginUser : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/users/login", async (LoginCommand command, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(command, ct);

                if (!response.IsSuccess)
                    return Results.BadRequest(response.ErrorMessage);

                return Results.Ok(response.AccessToken);
            });
        }
    }
}
