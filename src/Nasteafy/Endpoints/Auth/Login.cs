using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Auth.Commands.Login;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Auth
{
    public sealed class LoginEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/auth/login", async (LoginCommand command, HttpResponse http, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(command, ct);

                if (!response.IsSuccess)
                    return response.ToApiError();

                http.Cookies.Append("refreshToken", response.Value.RefreshToken!, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

                return Results.Ok(response.Value.AccessToken);

            })
              .Produces<string>(StatusCodes.Status200OK)
              .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
