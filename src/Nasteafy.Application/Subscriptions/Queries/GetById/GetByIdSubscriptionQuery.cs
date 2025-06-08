using FluentResults;
using MediatR;
using Nasteafy.Application.Abstractions.Data;
using Nasteafy.Application.Subscriptions.Queries.GetAll;

namespace Nasteafy.Application.Subscriptions.Queries.GetById
{
    public record GetByIdSubscriptionQuery(Guid SubscriptionId) : IRequest<Result<GetSubscriptionDto?>>;

    public class GetByIdSubscriptionQueryHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetByIdSubscriptionQuery, Result<GetSubscriptionDto?>>
    {
        public async Task<Result<GetSubscriptionDto?>> Handle(GetByIdSubscriptionQuery req, CancellationToken ct)
        {
            var subscription = await unitOfWork.Subscriptions
                .GetByIdAsync(req.SubscriptionId, ct);

            return new GetSubscriptionDto(subscription!.Id, subscription.Type.Name, subscription.Description, subscription.Price);
        }
    }
}
