using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Subscriptions.Queries.GetAll;
using Nasteafy.Application.Subscriptions.Queries.GetById;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Subscriptions.GetById
{
    public sealed class GetByIdSubscriptionEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapGet("api/subscriptions/{Id:guid}", async ([FromRoute] Guid Id, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(new GetSubscriptionByIdQuery(Id), ct);

                return response.IsSuccess
                    ? Results.Ok(response.Value)
                    : response.ToApiError();
            })
            .Produces<GetSubscriptionDto>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest); 
        }
    }
}
