using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Persistence.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasteafy.Infrastructure.Database.Configurations.Tracks
{
    public class AlbumConfiguration : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.ToTable("Albums", schema: SchemaConstants.Music);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.ReleaseDate)
                .IsRequired();

            builder.Property(x => x.CoverUrl)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.HasMany(x => x.AlbumArtists)
                .WithOne(a => a.Album)
                .HasForeignKey(a => a.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Tracks)
                .WithOne(t => t.Album)
                .HasForeignKey(t => t.AlbumId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
