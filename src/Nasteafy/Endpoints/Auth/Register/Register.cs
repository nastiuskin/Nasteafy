using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Auth.Commands.Register;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;
using System.Web.Http;

namespace Nasteafy.Endpoints.Auth.Register
{
    public sealed class RegisterEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/auth/register", async ([FromBody] RegisterRequest request, ISender sender, CancellationToken ct) =>
            {
                var command = new RegisterCommand(request.Email, request.Password);
                var response = await sender.Send(command, ct);

                return response.IsSuccess
                    ? Results.Ok()
                    : response.ToApiError();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
