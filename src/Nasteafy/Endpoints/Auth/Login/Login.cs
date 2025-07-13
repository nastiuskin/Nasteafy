using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Auth.Commands.Login;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;
using System.Web.Http;

namespace Nasteafy.Endpoints.Auth.Login
{
    public sealed class LoginEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/auth/login", async (
                [FromBody] LoginRequest request,
                HttpResponse http,
                ISender sender,
                CancellationToken ct) =>
            {
                var command = new LoginCommand(request.Email, request.Password);
                var response = await sender.Send(command, ct);

                if (!response.IsSuccess)
                {
                    return response.ToApiError();
                }

                http.Cookies.Append("refreshToken", response.Value.RefreshToken.Token!, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = response.Value.RefreshToken.ExpiresAt
                });

                return Results.Ok(response.Value.AccessToken);

            })
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
