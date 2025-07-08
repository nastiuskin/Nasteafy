using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Helpers;

namespace Nasteafy.Application.Subscriptions.Commands.Update
{
    public record UpdateSubscriptionCommand(
        Guid Id,
        string Description,
        decimal Price,
        int DurationInDays) : IRequest<Result>, ITransactionalCommand;

    public class UpdateSubscriptionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateSubscriptionCommand, Result>
    {

        public async Task<Result> Handle(UpdateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = await unitOfWork.Subscriptions.GetByIdAsync(request.Id, cancellationToken);

            subscription!.Description = request.Description;
            subscription.Price = request.Price;
            subscription.DurationInDays = request.DurationInDays;

            await unitOfWork.Subscriptions.UpdateAsync(subscription, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
