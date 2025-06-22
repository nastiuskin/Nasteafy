using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Auth.Commands.Register;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

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
                    return response.ToApiError();

                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
