using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Nasteafy.Abstractions;
using Nasteafy.Application.Subscriptions.Commands;

namespace Nasteafy.Endpoints.Subscriptions
{
    public sealed class AddSubscriptionToUserEndpoint : IEndpoint
    {
        [Authorize]
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/users/subscriptions", async (ActivateSubscriptionCommand command, ISender sender, CancellationToken ct) =>
            {
                //extract userId from claims
                var response = await sender.Send(command, ct);

                if (response.IsFailed)
                    return Results.BadRequest(response.Errors.Select(e => e.Message));

                return Results.Ok("Subscription added successfully");
            });
        }
    }
}
