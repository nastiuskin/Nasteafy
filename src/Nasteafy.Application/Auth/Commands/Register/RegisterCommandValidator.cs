using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Application.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator(UserManager<User> userManager)
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MustAsync(async (email, ct) =>
                 {
                     var existingUser = await userManager.FindByEmailAsync(email);
                     return existingUser == null;
                 }).WithMessage("User already exists.");

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("Password is required.");
        }
    }
}
