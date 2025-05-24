using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Infrastructure.Database.Configurations.Users
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "auth");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Artist)
             .WithOne()
             .HasForeignKey<Artist>(a => a.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.UserSubscriptions)
                .WithOne(us => us.User)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Playlists)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId) 
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
