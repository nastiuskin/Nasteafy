using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Helpers;
using Nasteafy.Domain.Entities.Subscriptions;

namespace Nasteafy.Application.Subscriptions.Queries.GetAll
{
    public record GetAllSubscriptionsQuery() : IRequest<Result<GetAllSubscriptionsResponse>>;

    public class GetAllSubscriptionsQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserProvider userProvider,
        IDateTimeService dateTimeService)
        : IRequestHandler<GetAllSubscriptionsQuery, Result<GetAllSubscriptionsResponse>>
    {
        public async Task<Result<GetAllSubscriptionsResponse>> Handle(GetAllSubscriptionsQuery req, CancellationToken ct)
        {
            var userId = userProvider.GetUserId();

            var subscriptionsQuery = unitOfWork.Subscriptions.GetAll();

            if (userId != Guid.Empty)
            {
                var userSubscriptions = await unitOfWork.Subscriptions.GetAllByUserIdAsync(userId, ct);

                var now = dateTimeService.UtcNow;

                var alreadyActivetedTrial = userSubscriptions.Any(us =>
                   us.Subscription.Type == SubscriptionType.Trial &&
                   us.EndDate < now);

                if (alreadyActivetedTrial)
                {
                    subscriptionsQuery = subscriptionsQuery.Where(s => s.Type != SubscriptionType.Trial);
                }
            }

            var subscriptions = await subscriptionsQuery
                .Select(x => new GetSubscriptionDto(
                    x.Id,
                    x.Type.Name,
                    x.Description,
                    x.Price))
                .ToListAsync(ct);

            var response = new GetAllSubscriptionsResponse { Subscriptions = subscriptions };

            return Result.Ok(response);
        }
    }
}