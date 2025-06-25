using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Tracks.Commands.AddTrack;

namespace Nasteafy.Application.Subscriptions.Commands.Cancel
{
    public record CancelActiveSubscriptionCommand : IRequest<Result>;

    public class CancelSubscriptionCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserProvider userProvider)
        : IRequestHandler<CancelActiveSubscriptionCommand, Result>
    {
        public async Task<Result> Handle(CancelActiveSubscriptionCommand request, CancellationToken ct)
        {
            var userId = userProvider.GetUserId();

            if (userId == null  || userId == Guid.Empty)
                return Result.Fail("UserId not found")
                     .LogIfFailed<CancelSubscriptionCommandHandler>(); 

            ///await unitOfWork.Subscriptions.CancelActiveSubscriptionAsync(userId.Value, ct);
            await unitOfWork.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }
}
