using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Auth.Commands.Logout;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Auth.Logout
{
    public sealed class LogoutEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/auth/logout", async (ISender sender, HttpResponse http, CancellationToken ct) =>
            {
                var response = await sender.Send(new LogoutCommand(), ct);

                if (!response.IsSuccess)
                {
                    return response.ToApiError();
                }                    

                http.Cookies.Delete("refreshToken", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Path = "/",
                });

                return Results.Ok();
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
