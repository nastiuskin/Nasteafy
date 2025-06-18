using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Auth;

namespace Nasteafy.Application.Auth.Commands.Register
{
    public record RegisterCommand(string Email, string Password) : IRequest<Result>;


    public class RegisterCommandHandler(
        IAuthenticationService authenticationService)
        : IRequestHandler<RegisterCommand, Result>
    {
        public async Task<Result> Handle(RegisterCommand request, CancellationToken ct)
        {
            return await authenticationService.RegisterAsync(request.Email, request.Password);
        }
    }
}
