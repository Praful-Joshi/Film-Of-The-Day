using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FilmOfTheDay.Infrastructure.Data.Seeders
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            // Skip if there are already Users
            if (await dbContext.Users.AnyAsync())
                return;

            var users = new List<User>();
            // Add 10 users
            for (int i = 1; i <= 10; i++)
            {
                users.Add(new User
                {
                    Username = "user" + i,
                    Email = "user" + i + "@example.com",
                    PasswordHash = "hashedpassword",
                });
            }
            await dbContext.Users.AddRangeAsync(users);
            await dbContext.SaveChangesAsync();
            Console.WriteLine("✅ Seeded 10 Users.");
        }
    }
}