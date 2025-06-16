using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Persistence.Constants;

namespace Nasteafy.Infrastructure.Database.Configurations.Tracks
{
    public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
            builder.ToTable("Playlists", schema: SchemaConstants.Music);

            builder.HasKey(x => x.Id);  

            builder.Property(x => x.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.CoverUrl)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.HasOne(x => x.User)
                .WithMany(u => u.Playlists)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.PlaylistTracks)
                .WithOne(pt => pt.Playlist)
                .HasForeignKey(x => x.PlaylistId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
