using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Auth.Commands.Logout;

namespace Nasteafy.Endpoints.Auth
{
    public sealed class LogoutEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/auth/logout", async (ISender sender, HttpResponse http, CancellationToken ct) =>
            {
                var response = await sender.Send(new LogoutCommand(), ct);

                if (!response.IsSuccess)
                    return Results.BadRequest(response.Reasons.First().Message);

                http.Cookies.Delete("refreshToken", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Path = "/", 
                });

                return Results.Ok();
            });
        }
    }
}
