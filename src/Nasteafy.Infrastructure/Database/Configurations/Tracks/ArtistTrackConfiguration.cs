using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nasteafy.Domain.Entities;

namespace Nasteafy.Infrastructure.Database.Configurations.Tracks
{
    public class ArtistTrackConfiguration : IEntityTypeConfiguration<ArtistTrack>
    {
        public void Configure(EntityTypeBuilder<ArtistTrack> builder)
        {
            builder.ToTable("ArtistTracks", schema: "music");

            builder.HasKey(x => new {x.TrackId, x.ArtistId});

            builder.HasOne(x => x.Artist)
                .WithMany(a => a.ArtistTracks)
                .HasForeignKey(x => x.ArtistId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Track)
                .WithMany(t => t.ArtistTracks)
                .HasForeignKey(x => x.TrackId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
