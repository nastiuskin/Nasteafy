using MediatR;
using Microsoft.AspNetCore.Authorization;
using Nasteafy.Abstractions;
using Nasteafy.Application.Subscriptions.Commands;

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
                    return Results.BadRequest(response.Errors.Select(e => e.Message));

                return Results.Ok("Subscription added successfully");
            });
        }
    }
}
