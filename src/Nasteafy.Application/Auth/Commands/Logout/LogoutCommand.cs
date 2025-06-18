using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Auth;

namespace Nasteafy.Application.Auth.Commands.Logout
{
    public record LogoutCommand : IRequest<Result>;

    public class LogoutCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<LogoutCommand, Result>
    {
        public async Task<Result> Handle(LogoutCommand command, CancellationToken ct)
        {
            return await authenticationService.LogoutAsync();
        }
    }
}
