using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Auth;

namespace Nasteafy.Application.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponse>>;


    public class LoginCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<LoginCommand, Result<AuthResponse>>
    {
        public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken ct)
        {
            return await authenticationService.PasswordSignInAsync(request.Email, request.Password);
        }
    }
}
