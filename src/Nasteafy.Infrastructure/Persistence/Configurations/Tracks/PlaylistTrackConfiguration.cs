using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Persistence.Constants;

namespace Nasteafy.Infrastructure.Database.Configurations.Tracks
{
    public class PlaylistTrackConfiguration : IEntityTypeConfiguration<PlaylistTrack>
    {
        public void Configure(EntityTypeBuilder<PlaylistTrack> builder)
        {
            // AlbumArtists -> ArtistTracks -> PlaylistTracks. Missing s in table name.
            builder.ToTable("PlaylistTracks", schema: SchemaConstants.Music);

            builder.HasKey(x => new { x.PlaylistId, x.TrackId });

            builder.Property(x => x.Order).IsRequired();

            builder.HasOne(x => x.Playlist)
               .WithMany(p => p.PlaylistTracks)
               .HasForeignKey(x => x.PlaylistId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Track)
              .WithMany(t => t.PlaylistTracks)
              .HasForeignKey(x => x.TrackId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
