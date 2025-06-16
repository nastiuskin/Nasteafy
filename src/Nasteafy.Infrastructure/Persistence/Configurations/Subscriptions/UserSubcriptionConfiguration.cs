using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nasteafy.Domain.Entities.Subscriptions;
using Nasteafy.Persistence.Constants;

namespace Nasteafy.Infrastructure.Database.Configurations.Subscriptions
{
    public class UserSubcriptionConfiguration : IEntityTypeConfiguration<UserSubscription>
    {
        public void Configure(EntityTypeBuilder<UserSubscription> builder) 
        {
            builder.ToTable("UserSubscriptions", schema: SchemaConstants.Subscription);

            builder.HasKey(x => new {x.UserId, x.SubscriptionId});

            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired();

            builder.HasOne(x => x.Subscription)
                .WithMany(s => s.UserSubscriptions)
                .HasForeignKey(x => x.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                .WithMany(u => u.UserSubscriptions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
