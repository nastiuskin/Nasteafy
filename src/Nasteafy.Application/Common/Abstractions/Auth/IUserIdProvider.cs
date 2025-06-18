namespace Nasteafy.Application.Common.Abstractions.Auth
{
    public interface IUserIdProvider
    {
        Guid? GetUserId();
    }
}
