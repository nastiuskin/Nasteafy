using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Persistence.Constants;

namespace Nasteafy.Infrastructure.Database.Configurations.Tracks
{
    public class AlbumArtistConfiguration : IEntityTypeConfiguration<AlbumArtist>
    {
        public void Configure(EntityTypeBuilder<AlbumArtist> builder)
        {
            builder.ToTable("AlbumArtists", schema: SchemaConstants.Music);

            builder.HasKey(x => new { x.AlbumId, x.ArtistId });

            builder.HasOne(x => x.Artist)
                .WithMany(a => a.AlbumArtists)
                .HasForeignKey(x => x.ArtistId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Album)
                .WithMany(a => a.AlbumArtists)
                .HasForeignKey(x => x.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
