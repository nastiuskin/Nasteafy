namespace Nasteafy.Application.Abstractions.Auth
{
    public interface IUserIdProvider
    {
        Guid? GetUserId();
    }
}
