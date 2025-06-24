using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Subscriptions.Commands.Cancel;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Subscriptions
{
    public sealed class CancelSubscriptionEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/subscriptions/cancel", async (ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(new CancelActiveSubscriptionCommand(), ct);

                if (response.IsFailed)
                    return response.ToApiError();

                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
