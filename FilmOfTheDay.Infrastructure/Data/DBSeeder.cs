using FilmOfTheDay.Core.Entities;
using Microsoft.EntityFrameworkCore;
using FilmOfTheDay.Infrastructure.Data.Seeders;

namespace FilmOfTheDay.Infrastructure.Data
{
    public static class DBSeeder
    {
        public static async Task SeedDataToDB(ApplicationDbContext dbContext)
        {
            // await UserSeeder.SeedAsync(dbContext);
            // await PostSeeder.SeedAsync(dbContext); 
            // await PostSeeder.SeedPostsToUser(dbContext, "prafuljoshi1010@gmail.com");
        }
    }
}
