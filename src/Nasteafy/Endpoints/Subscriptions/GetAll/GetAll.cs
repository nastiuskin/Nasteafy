using MediatR;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Subscriptions.Queries.GetAll;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Subscriptions.GetAll
{
    public sealed class GetAllSubscriptionsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/subscriptions", async (ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(new GetAllSubscriptionsQuery(), ct);

                 return response.IsSuccess
                    ? Results.Ok(response.Value)
                    : response.ToApiError();
            })
            .Produces<GetAllSubscriptionsResponse>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
