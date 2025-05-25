using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Abstractions.Data;
using Nasteafy.Domain.Entities;
using Nasteafy.Domain.Entities.Subscriptions;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Domain.Entities.Users;
using System.Reflection;

namespace Nasteafy.Infrastructure.Database
{
    public sealed class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options), IUnitOfWork
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
        public DbSet<AlbumArtist> AlbumArtists { get; set; }
        public DbSet<ArtistTrack> ArtistTracks { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
