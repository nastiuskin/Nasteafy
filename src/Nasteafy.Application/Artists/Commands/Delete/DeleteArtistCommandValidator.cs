using FluentValidation;
using Nasteafy.Application.Common.Abstractions.Data;
using System.Security.Cryptography.X509Certificates;

namespace Nasteafy.Application.Artists.Commands.Delete
{
    public class DeleteArtistCommandValidator : AbstractValidator<DeleteArtistCommand>
    {
        public DeleteArtistCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.ArtistId)
                .NotEmpty()
                .WithMessage("Artist ID must not be empty.")
                .MustAsync(unitOfWork.Artists.ExistsAsync)
                .WithMessage("Artist does not exist.");
        }
    }
}
