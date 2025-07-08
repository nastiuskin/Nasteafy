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

            //Move this into BaseEntityConfiguration so that all entities have guid generated automatically
            builder.HasKey(x => x.Id);

            //Move this into BaseEntityConfiguration so that all entities have guid generated automatically
            builder.Property(e => e.Id)
                  .HasDefaultValueSql("NEWID()")
                  .ValueGeneratedOnAdd();

            builder.Property(x => x.AvatarUrl)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.OwnsOne(x => x.RefreshToken, rt =>
            {
                rt.Property(r => r.Token)
                  .HasColumnName("RefreshToken")
                  .HasMaxLength(500);

                rt.Property(r => r.ExpiresAt)
                  .HasColumnName("RefreshTokenExpires");
            });

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
