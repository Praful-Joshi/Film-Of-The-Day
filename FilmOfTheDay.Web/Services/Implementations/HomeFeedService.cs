using FilmOfTheDay.Core.Entities;
using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Web.Models.Home;
using FilmOfTheDay.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FilmOfTheDay.Web.Services.Implementations;
public class HomeFeedService : IHomeFeedService
{
    private readonly ApplicationDbContext _dbContext;

    public HomeFeedService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HomeFeedViewModel> GetHomeFeedAsync(int userId)
    {
        // Get all friend user IDs
        var friendIds = await _dbContext.Friendships
            .Where(f => (f.SenderId == userId || f.ReceiverId == userId) && f.Status == FriendshipStatus.Accepted)
            .Select(f => f.SenderId == userId ? f.ReceiverId : f.SenderId)
            .Distinct()
            .ToListAsync();

        // Include the current user
        friendIds.Add(userId);

        var feedItems = await _dbContext.FilmPosts
            .Where(fi => friendIds.Contains(fi.UserId))
            .OrderByDescending(fi => fi.CreatedAt)
            .Select(fi => new FeedItemViewModel
            {
                Id = fi.Id,
                Title = fi.Title,
                Description = fi.Description,
                CreatedAt = fi.CreatedAt,
                ImageUrl = fi.ImageUrl,
                UserName = fi.User.Username,
                UserID = fi.UserId,
            })
            .ToListAsync();

        return new HomeFeedViewModel
        {
            FeedItems = feedItems
        };
    }
}