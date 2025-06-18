using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Application.Auth.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        private readonly UserManager<User> _userManager;

        public LoginCommandValidator(UserManager<User> userManager)
        {
            _userManager = userManager;

            RuleFor(x => x.Email)
             .NotEmpty().WithMessage("Email is required.")
             .EmailAddress().WithMessage("Invalid email format.")
             .MustAsync(UserExists)
             .WithMessage("User not found");

            RuleFor(x => x.Password)
             .NotEmpty()
             .WithMessage("Password is required");
        }

        private async Task<bool> UserExists(string email, CancellationToken ct)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
