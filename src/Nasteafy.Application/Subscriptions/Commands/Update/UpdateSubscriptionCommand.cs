using FluentResults;
using MediatR;
using Nasteafy.Application.Abstractions.Data;

namespace Nasteafy.Application.Subscriptions.Commands.Update
{
    public record UpdateSubscriptionCommand(
        Guid Id,
        string Description,
        decimal Price,
        int DurationInDays) : IRequest<Result<Guid>>;

    public class UpdateSubscriptionCommandHandler : IRequestHandler<UpdateSubscriptionCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSubscriptionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(UpdateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(request.Id, cancellationToken);

            subscription!.Description = request.Description;
            subscription.Price = request.Price;
            subscription.DurationInDays = request.DurationInDays;

            await _unitOfWork.Subscriptions.UpdateAsync(subscription, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Ok(subscription.Id);
        }
    }
}
