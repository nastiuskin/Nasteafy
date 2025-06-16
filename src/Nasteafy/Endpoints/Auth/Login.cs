using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Auth.Commands.Login;

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
                    return Results.BadRequest(response.ErrorMessage);

                http.Cookies.Append("refreshToken", response.RefreshToken!, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, 
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

                return Results.Ok(response.AccessToken);

            }).Produces<string>(StatusCodes.Status200OK)
              .Produces<string>(StatusCodes.Status400BadRequest);
        }
    }
}
