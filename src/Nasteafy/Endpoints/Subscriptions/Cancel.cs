using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Subscriptions.Commands.Cancel;
using System.Web.Http;

namespace Nasteafy.Endpoints.Subscriptions
{
    public sealed class CancelSubscriptionEndpoint : IEndpoint
    {
        [Authorize]
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/subscriptions/cancel", async (ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(new CancelActiveSubscriptionCommand(), ct);

                if (response.IsFailed)
                    return Results.BadRequest(response.Errors.Select(e => e.Message));

                return Results.Ok("Subscription canceled successfully");
            });
        }
    }
}
