using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Application.Subscriptions.Queries.GetAll
{
    public record GetAllSubscriptionsQuery() : IRequest<Result<GetAllSubscriptionsResponse>>;

    //public class GetAllSubscriptionsQueryHandler(IUnitOfWork unitOfWork)
    //    : IRequestHandler<GetAllSubscriptionsQuery, Result<GetAllSubscriptionsResponse>>
    //{
    //    public async Task<Result<GetAllSubscriptionsResponse>> Handle(GetAllSubscriptionsQuery req, CancellationToken ct)
    //    {
    //        var subscriptions = await unitOfWork.Subscriptions
    //            .GetAll()
    //            .Select(x => new GetSubscriptionDto(
    //                x.Id,
    //                x.Type.Name,
    //                x.Description,
    //                x.Price))
    //            .ToListAsync(ct);

    //        var response = new GetAllSubscriptionsResponse { Subscriptions = subscriptions };

    //        return Result.Ok(response);
    //    }
    //}
}