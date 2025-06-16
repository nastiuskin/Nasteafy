using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Auth.Commands.Login;
using Nasteafy.Application.Auth.Commands.RefreshToken;

namespace Nasteafy.Endpoints.Auth
{
    public sealed class RefreshTokenEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/auth/refresh", async (HttpResponse response, HttpRequest request, ISender sender, CancellationToken ct) =>
            {
                if (!request.Cookies.TryGetValue("refreshToken", out var refreshToken) || string.IsNullOrWhiteSpace(refreshToken))
                    return Results.Unauthorized();

                var result = await sender.Send(new RefreshTokenCommand(refreshToken), ct);

                if (!result.IsSuccess)
                    return Results.Unauthorized();

                response.Cookies.Append("refreshToken", result.RefreshToken!, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

                return Results.Ok(result.AccessToken);
            })
        .Produces<string>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
        }
    }
}
