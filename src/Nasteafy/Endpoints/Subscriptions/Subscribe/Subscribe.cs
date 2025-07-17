using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Subscriptions.Commands;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Subscriptions.Subscribe
{
    public sealed class SubscribeUserEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/users/subscriptions/{subscriptionId:guid}/subscribe", async ([FromRoute] Guid subscriptionId, ISender sender,
                CancellationToken ct) =>
            {
                var command = new SubscribeUserCommand(subscriptionId);
                var response = await sender.Send(command, ct);

                return response.IsSuccess
                    ? Results.Ok()
                    : response.ToApiError();
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
