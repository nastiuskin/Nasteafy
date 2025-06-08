using FluentResults;
using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Subscriptions.Queries.GetAll;

namespace Nasteafy.Endpoints.Subscriptions
{
    public sealed class GetAllSubscriptionsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/subscriptions", async (ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(new GetAllSubscriptionsQuery(), ct);

                if (response.IsFailed)
                    return Results.BadRequest(response.Errors.Select(e => e.Message));

                return Results.Ok(response.Value);
            });
        }
    }
}
