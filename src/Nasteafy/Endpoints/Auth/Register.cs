using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Auth.Commands.Register;

namespace Nasteafy.Endpoints.Auth
{
    public sealed class RegisterEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/auth/register", async (RegisterCommand command, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(command, ct);

                if (!response.IsSuccess)
                    return Results.BadRequest(response.Reasons.First().Message);

                return Results.Ok("Registration successful");
            });
        }
    }
}
