namespace Nasteafy.Application.Common.Abstractions.Auth
{
    public interface ICurrentUserProvider
    {
        Guid GetUserId();
    }
}
