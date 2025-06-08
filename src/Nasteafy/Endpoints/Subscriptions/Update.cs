using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Subscriptions.Commands.Update;
using System.Web.Http;

namespace Nasteafy.Endpoints.Subscriptions
{
    public sealed class UpdateSubscriptionEndpoint : IEndpoint
    {
        [Authorize(Roles = "Admin")]
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPut("api/subscriptions", async (UpdateSubscriptionCommand req, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(req, ct);

                if (response.IsFailed)
                    return Results.BadRequest(response.Errors.Select(e => e.Message));

                return Results.Ok(response.Value);
            });
        }
    }
}
