using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Subscriptions.Queries.GetAll;
using Nasteafy.Application.Subscriptions.Queries.GetById;

namespace Nasteafy.Endpoints.Subscriptions
{
    public sealed class GetByIdSubscriptionEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/subscriptions/{Id:guid}", async ([FromRoute] Guid Id, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(new GetSubscriptionByIdQuery(Id), ct);

                if (response.IsFailed)
                    return Results.BadRequest(response.Errors.Select(e => e.Message));

                return Results.Ok(response.Value);
            });
        }
    }
}
