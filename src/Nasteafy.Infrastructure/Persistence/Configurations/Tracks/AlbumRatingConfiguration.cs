using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Persistence.Constants;

namespace Nasteafy.Infrastructure.Persistence.Configurations.Tracks
{
    public class AlbumRatingConfiguration : IEntityTypeConfiguration<AlbumRating>
    {
        public void Configure(EntityTypeBuilder<AlbumRating> builder)
        {
            builder.ToTable("AlbumRatings", schema: SchemaConstants.Music);

            builder.HasKey(x => new { x.AlbumId, x.UserId });

            builder.HasOne(x => x.Album)
                .WithMany(a => a.Ratings)
                .HasForeignKey(x => x.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany(u => u.AlbumRatings)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
