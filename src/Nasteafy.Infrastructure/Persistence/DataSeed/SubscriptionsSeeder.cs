using Microsoft.Extensions.DependencyInjection;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain.Entities.Subscriptions;

namespace Nasteafy.Infrastructure.Persistence.DataSeed
{
    public static class SubscriptionsSeeder
    {
        // No need for ct in parameters since you don't pass it outside. 
        public static async Task SeedAsync(IServiceProvider serviceProvider, CancellationToken ct = default)
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
                    DurationInDays = 365,
                    UserSubscriptions = []
                },
                new()
                {
                    Type = SubscriptionType.Trial,
                    Description = "15-day free trial with premium features",
                    Price = 0,
                    DurationInDays = 15,
                    UserSubscriptions = []
                },
                new()
                {
                    Type = SubscriptionType.Premium,
                    Description = "Full access with no ads",
                    Price = 9.99m,
                    DurationInDays = 30,
                    UserSubscriptions = []
                },
                new()
                {
                    Type = SubscriptionType.Artist,
                    Description = "Upload and manage your own music",
                    Price = 19.99m,
                    DurationInDays = 30,
                    UserSubscriptions = []
                }
            };

            foreach (var subscription in subscriptions)
            {
                // You are making a lot of requests here, better to transform this query into contains and outside of this foreach 
                var existingSubscription = await unitOfWork.Subscriptions.GetByTypeAsync(subscription.Type, ct);

                if (existingSubscription is null)
                {
                    await unitOfWork.Subscriptions.AddAsync(subscription, ct);
                }
            }

            await unitOfWork.SaveChangesAsync(ct);
        }
    }
}
