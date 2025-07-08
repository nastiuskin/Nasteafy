using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace Nasteafy.Extensions
{
    public static class ErrorFormattingExtensions
    {
        public static string ToErrorMessage(this IEnumerable<IError> errors)
        {
            return string.Join("; ", errors.Select(e => e.Message));
        }

        public static string ToErrorMessage(this IEnumerable<string> errors)
        {
            return string.Join("; ", errors);
        }

        public static string ToErrorMessage(this IEnumerable<IdentityError> errors)
        {
            return string.Join("; ", errors.Select(e => e.Description));
        }
    }
}
