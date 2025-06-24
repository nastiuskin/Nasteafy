using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Subscriptions.Commands;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Subscriptions
{
    public sealed class SubscribeUserEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/users/subscribe", async (SubscribeUserCommand command, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(command, ct);

                if (response.IsFailed)
                    return response.ToApiError();

                return Results.Ok();
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
