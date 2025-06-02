using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Users.Commands.Register;

namespace Nasteafy.Endpoints.Users
{
    public sealed class RegisterUser : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/users/register", async (RegisterCommand command, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(command, ct);

                if (!response.IsSuccess)
                    return Results.BadRequest(response.ErrorMessage);

                return Results.Ok(response.AccessToken);
            });
        }
    }
}
