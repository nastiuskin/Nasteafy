using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Subscriptions.Commands.Update;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Subscriptions
{
    public sealed class UpdateSubscriptionEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPut("api/subscriptions", async (UpdateSubscriptionCommand req, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(req, ct);

                if (response.IsFailed)
                    return response.ToApiError();

                return Results.Ok();
            })
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
