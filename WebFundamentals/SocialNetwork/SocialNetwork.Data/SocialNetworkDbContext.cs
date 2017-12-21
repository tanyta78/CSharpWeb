namespace SocialNetwork.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Models;

    public class SocialNetworkDbContext : DbContext
    {
        public SocialNetworkDbContext()
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<AlbumPicture> AlbumPictures { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<AlbumTag> AlbumTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(ServerConfig.ConfigurationString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Friendship>()
                .HasKey(f => new {f.FromUserId, f.ToUserId});

            builder
                .Entity<User>()
                .HasMany(u => u.FromFriends)
                .WithOne(f => f.FromUser)
                .HasForeignKey(f => f.FromUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<User>()
                .HasMany(u => u.ToFriends)
                .WithOne(f => f.ToUser)
                .HasForeignKey(f => f.ToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<AlbumPicture>()
                .HasKey(ap => new {ap.AlbumId, ap.PictureId});

            builder.Entity<AlbumPicture>()
                   .HasOne<Picture>(ap => ap.Picture)
                   .WithMany(p => p.Albums)
                   .HasForeignKey(ap => ap.PictureId);

            builder.Entity<AlbumPicture>()
                   .HasOne<Album>(ap => ap.Album)
                   .WithMany(p => p.Pictures)
                   .HasForeignKey(ap => ap.AlbumId);

            builder.Entity<Album>()
                   .HasOne<User>(a => a.Owner)
                   .WithMany(o => o.Albums)
                   .HasForeignKey(a => a.OwnerId);

            builder.Entity<AlbumTag>()
                   .HasKey(at => new {at.AlbumId, at.TagId});

            builder.Entity<AlbumTag>()
                   .HasOne<Album>(at => at.Album)
                   .WithMany(a => a.Tags)
                   .HasForeignKey(at => at.AlbumId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AlbumTag>()
                   .HasOne<Tag>(at => at.Tag)
                   .WithMany(t => t.Albums)
                   .HasForeignKey(at => at.TagId)
                   .OnDelete(DeleteBehavior.Restrict);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var serviceProvider = this.GetService<IServiceProvider>();
            var items = new Dictionary<object, object>();

            foreach (var entry in this.ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added) || (e.State == EntityState.Modified)))

            {
                var entity = entry.Entity;
                var context = new ValidationContext(entity, serviceProvider, items);
                var results = new List<ValidationResult>();

                if (Validator.TryValidateObject(entity, context, results, true) == false)

                {
                    foreach (var result in results)
                    {
                        if (result != ValidationResult.Success)
                        {
                            throw new ValidationException(result.ErrorMessage);
                        }
                    }
                }
            }
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
    }
}
