using FilmOfTheDay.Core.Entities;
using FilmOfTheDay.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FilmOfTheDay.Infrastructure.Data.Seeders
{
    public static class PostSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            // Skip if there are already FilmPosts
            if (await dbContext.FilmPosts.AnyAsync())
                return;

            // Retrieve users from DB to assign posts to
            var users = await dbContext.Users.ToListAsync();
            if (!users.Any())
            {
                Console.WriteLine("⚠️ No users found, skipping post seeding.");
                return;
            }

            var random = new Random();
            var posts = new List<FilmPost>();

            for (int i = 1; i <= 10; i++)
            {
                var user = users[random.Next(users.Count)];
                posts.Add(new FilmPost
                {
                    UserId = user.Id,
                    Title = $"Film Post #{i}",
                    Description = $"A review or thought about film #{i}",
                    CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30))
                });
            }

            await dbContext.FilmPosts.AddRangeAsync(posts);
            await dbContext.SaveChangesAsync();
            Console.WriteLine("✅ Seeded 10 FilmPosts.");
        }
        public static async Task SeedPostsToUser(ApplicationDbContext dbContext, String userEmail)
        {
            var random = new Random();
            var posts = new List<FilmPost>();
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user != null)
            {
                //skip if user already has posts
                if (await dbContext.FilmPosts.AnyAsync(p => p.UserId == user.Id))
                {
                    Console.WriteLine($"⚠️ User {user.Username} already has posts, skipping post seeding.");
                    return;
                }

                for (int i = 1; i <= 5; i++)
                {
                    posts.Add(new FilmPost
                    {
                        UserId = user.Id,
                        Title = $"Film Post #{i} by {user.Username}",
                        Description = $"A review or thought about film #{i} by {user.Username}",
                        CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30))
                    });
                }
                await dbContext.FilmPosts.AddRangeAsync(posts);
                await dbContext.SaveChangesAsync();
                Console.WriteLine($"✅ Seeded 5 FilmPosts for user {user.Username}.");
            }
            
        }
    }
}
