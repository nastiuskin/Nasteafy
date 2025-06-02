using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Persistence.Constants;

namespace Nasteafy.Infrastructure.Database.Configurations.Tracks
{
    public class TrackConfiguration : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> builder)
        {
            builder.ToTable("Tracks", schema: SchemaConstants.Music);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Duration).IsRequired();
            builder.Property(x => x.FilePath).HasMaxLength(255).IsRequired(); 

            builder.HasOne(x => x.Album)
                .WithMany(a => a.Tracks)
                .HasForeignKey(x => x.AlbumId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.PlaylistTracks)
                .WithOne(pt => pt.Track)
                .HasForeignKey(pt => pt.TrackId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.ArtistTracks)
                .WithOne(at => at.Track)
                .HasForeignKey(at => at.TrackId)
                .OnDelete(DeleteBehavior.Cascade);
        }   
    }
}
