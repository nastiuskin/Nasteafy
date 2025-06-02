using FluentResults;
using MediatR;
using Nasteafy.Application.Abstractions.Auth;

namespace Nasteafy.Application.Users.Commands.Register
{
    public record RegisterCommand(string Email, string Password) : IRequest<AuthResult>;


    public class RegisterCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<RegisterCommand, AuthResult>
    {
        public async Task<AuthResult> Handle(RegisterCommand request, CancellationToken ct)
        {
            return await authenticationService.RegisterAsync(request.Email, request.Password);
        }
    }
}
