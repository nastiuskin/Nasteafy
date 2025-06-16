using FluentResults;
using MediatR;
using Nasteafy.Application.Abstractions.Auth;
using Nasteafy.Application.Abstractions.Data;

namespace Nasteafy.Application.Subscriptions.Commands.Cancel
{
    public record CancelActiveSubscriptionCommand : IRequest<Result>;

    public class CancelSubscriptionCommandHandler(
        IUnitOfWork unitOfWork,
        IUserIdProvider userProvider)
        : IRequestHandler<CancelActiveSubscriptionCommand, Result>
    {
        public async Task<Result> Handle(CancelActiveSubscriptionCommand request, CancellationToken ct)
        {
            var userId = userProvider.GetUserId();

            if (userId == null  || userId == Guid.Empty)
                return Result.Fail("UserId not found");

            await unitOfWork.Subscriptions.CancelActiveSubscriptionAsync(userId.Value, ct);
            await unitOfWork.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }
}
