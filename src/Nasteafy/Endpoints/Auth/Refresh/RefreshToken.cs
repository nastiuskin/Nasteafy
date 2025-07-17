using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Auth.Commands.UseRefreshToken;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Auth.Refresh
{
    public sealed class RefreshTokenEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/auth/refresh", async (HttpResponse response, HttpRequest request, ISender sender, CancellationToken ct) =>
            {
                var refreshToken = request.Cookies["refreshToken"];

                var result = await sender.Send(new RefreshTokenCommand(refreshToken), ct);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : result.ToApiError(StatusCodes.Status401Unauthorized);

            })
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status401Unauthorized);
        }
    }
}
