using FluentResults;
using MediatR;
using Nasteafy.Application.Auth.Commands.Login;
using Nasteafy.Application.Common.Abstractions.Auth;

namespace Nasteafy.Application.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<Result<AuthResponse>>;

    public class RefreshTokenCommandHandler(IAuthenticationService authService)
     : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
    {
        public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            return await authService.RefreshTokenAsync(request.RefreshToken);
        }   
    }
}