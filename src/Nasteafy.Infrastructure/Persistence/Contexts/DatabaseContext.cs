using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nasteafy.Domain.Entities;
using Nasteafy.Domain.Entities.Subscriptions;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Domain.Entities.Users;
using Nasteafy.Persistence.Constants;

namespace Nasteafy.Infrastructure.Persistence.Contexts
{
    public sealed class DatabaseContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
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
            ApplyIdentityMapConfiguration(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        }

        private void ApplyIdentityMapConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims", SchemaConstants.Auth);
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins", SchemaConstants.Auth);
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens", SchemaConstants.Auth);
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles", SchemaConstants.Auth);
            modelBuilder.Entity<IdentityRole<Guid>>().ToTable("Roles", SchemaConstants.Auth);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims", SchemaConstants.Auth);
        }
    }
}
