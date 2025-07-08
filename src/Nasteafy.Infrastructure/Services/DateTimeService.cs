using Nasteafy.Application.Common.Abstractions.Helpers;

namespace Nasteafy.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
