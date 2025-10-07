using FilmOfTheDay.Core.Entities;
using Microsoft.EntityFrameworkCore;
namespace FilmOfTheDay.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Friendship
            modelBuilder.Entity<Friendship>(entity =>
            {
                entity.HasOne(f => f.User)
                    .WithMany()
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(f => f.Friend)
                    .WithMany()
                    .HasForeignKey(f => f.FriendId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // UserWatchlist
            modelBuilder.Entity<UserWatchlist>(entity =>
            {
                entity.HasOne(uw => uw.User)
                    .WithMany()
                    .HasForeignKey(uw => uw.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(uw => uw.FilmPost)
                    .WithMany()
                    .HasForeignKey(uw => uw.FilmPostId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // FilmPosts (if you still want cascade delete from User → FilmPosts)
            modelBuilder.Entity<FilmPost>(entity =>
            {
                entity.HasOne(fp => fp.User)
                    .WithMany()
                    .HasForeignKey(fp => fp.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Only one cascade path allowed
            });
        }


        public DbSet<User> Users { get; set; }
        public DbSet<FilmPost> FilmPosts { get; set; } = null!;
        public DbSet<Friendship> Friendships { get; set; } = null!;
        public DbSet<UserWatchlist> UserWatchlists { get; set; } = null!;
    }
}