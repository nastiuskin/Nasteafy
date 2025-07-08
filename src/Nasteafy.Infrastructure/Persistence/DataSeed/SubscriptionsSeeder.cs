using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain.Entities.Subscriptions;

namespace Nasteafy.Infrastructure.Persistence.DataSeed
{
    public static class SubscriptionsSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, CancellationToken ct)
        {
            using var scope = serviceProvider.CreateScope();

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var subscriptions = new List<Subscription>
            {
                new()
                {
                    Type = SubscriptionType.Free,
                    Description = "Free access with ads and limited features",
                    Price = 0,
                    DurationInDays = 365
                },
                new()
                {
                    Type = SubscriptionType.Trial,
                    Description = "15-day free trial with premium features",
                    Price = 0,
                    DurationInDays = 15
                },
                new()
                {
                    Type = SubscriptionType.Premium,
                    Description = "Full access with no ads",
                    Price = 9.99m,
                    DurationInDays = 30
                },
                new()
                {
                    Type = SubscriptionType.Artist,
                    Description = "Upload and manage your own music",
                    Price = 19.99m,
                    DurationInDays = 30
                }
            };

            var existingSubscriptions = await unitOfWork.Subscriptions
                .GetAll()
                .Select(x => x.Type)
                .ToHashSetAsync(ct);

            var newSubscriptions = subscriptions
                 .Where(x => !existingSubscriptions.Contains(x.Type))
                 .ToList();

            if (newSubscriptions.Any())
            {
                await unitOfWork.Subscriptions.AddRangeAsync(newSubscriptions, ct);
                await unitOfWork.SaveChangesAsync(ct);
            }
        }
    }
}
