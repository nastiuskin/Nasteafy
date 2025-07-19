using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Persistence.Constants;

namespace Nasteafy.Infrastructure.Persistence.Configurations.Tracks
{
    public class TrackLikeConfiguration : IEntityTypeConfiguration<TrackLike>
    {
        public void Configure(EntityTypeBuilder<TrackLike> builder)
        {
            builder.ToTable("TrackLikes", schema: SchemaConstants.Music);

            builder.HasKey(x => new { x.TrackId, x.UserId });

            builder.HasOne(x => x.Track)
                .WithMany(a => a.TrackLikes)
                .HasForeignKey(x => x.TrackId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany(u => u.TrackLikes)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
