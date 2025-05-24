using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nasteafy.Domain.Entities.Subscriptions;

namespace Nasteafy.Persistence.Database.Configurations.Subscriptions
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable("Subscriptions", schema: "subscriptions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.DurationInDays).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500).IsRequired();

            builder.HasMany(x => x.UserSubscriptions)
                .WithOne(us => us.Subscription)
                .HasForeignKey(us => us.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
