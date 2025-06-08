namespace Nasteafy.Application.Abstractions.Auth
{
    public interface IUserProvider
    {
        Guid GetUserId();
    }
}
