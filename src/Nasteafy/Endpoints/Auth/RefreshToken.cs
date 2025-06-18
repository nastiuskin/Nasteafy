using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Auth.Commands.RefreshToken;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

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
                    return result.ToApiError();

                response.Cookies.Append("refreshToken", result.Value.RefreshToken!, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

                return Results.Ok(result.Value.AccessToken);
            })
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
