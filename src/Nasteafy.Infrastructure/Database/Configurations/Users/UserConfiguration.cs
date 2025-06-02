using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Domain.Entities.Users;
using Nasteafy.Persistence.Constants;

namespace Nasteafy.Infrastructure.Database.Configurations.Users
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", schema: SchemaConstants.Auth);

            builder.HasKey(x => x.Id);

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
