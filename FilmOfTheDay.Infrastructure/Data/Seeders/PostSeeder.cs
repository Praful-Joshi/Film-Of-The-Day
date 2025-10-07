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
    }
}
