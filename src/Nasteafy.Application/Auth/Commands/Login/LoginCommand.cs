using MediatR;
using Nasteafy.Application.Abstractions.Auth;

namespace Nasteafy.Application.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<AuthResult>;


    public class LoginCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<LoginCommand, AuthResult>
    {
        public async Task<AuthResult> Handle(LoginCommand request, CancellationToken ct)
        {
            return await authenticationService.PasswordSignInAsync(request.Email, request.Password);
        }
    }
}
