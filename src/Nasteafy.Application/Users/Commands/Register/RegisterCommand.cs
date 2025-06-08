using MediatR;
using Nasteafy.Application.Abstractions.Auth;
using Nasteafy.Application.Abstractions.Data;
using Nasteafy.Domain.Entities.Subscriptions;

namespace Nasteafy.Application.Users.Commands.Register
{
    public record RegisterCommand(string Email, string Password) : IRequest<AuthResult>;


    public class RegisterCommandHandler(IUnitOfWork unitOfWork,
        IAuthenticationService authenticationService)
        : IRequestHandler<RegisterCommand, AuthResult>
    {
        public async Task<AuthResult> Handle(RegisterCommand request, CancellationToken ct)
        {
            return await authenticationService.RegisterAsync(request.Email, request.Password);
        }
    }
}
