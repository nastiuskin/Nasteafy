using MediatR;
using Nasteafy.Application.Abstractions.Auth;

namespace Nasteafy.Application.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResult>;

    public class RefreshTokenCommandHandler(IAuthenticationService authService)
     : IRequestHandler<RefreshTokenCommand, AuthResult>
    {
        public async Task<AuthResult> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            return await authService.RefreshTokenAsync(request.RefreshToken);
        }   
    }
}