using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Web.Models.Profile;
using FilmOfTheDay.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FilmOfTheDay.Web.Services.Implementations;
public class ProfileService : IProfileService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IConnectionService _connectionService;

    public ProfileService(ApplicationDbContext dbContext, IConnectionService connectionService)
    {
        _dbContext = dbContext;
        _connectionService = connectionService;
    }

    public async Task<ProfileViewModel?> GetProfileAsync(int profileUserId, ClaimsPrincipal currentUser)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == profileUserId);

        if (user == null)
            return null;

        var loggedInUserIdStr = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(loggedInUserIdStr) || !int.TryParse(loggedInUserIdStr, out var loggedInUserId))
            return null;

        // Fetch friendship state and counts
        var friendshipState = _connectionService.GetFriendshipState(loggedInUserId, profileUserId);
        var friendsCount = _connectionService.GetFriendsCount(profileUserId);

        // Fetch posts
        var posts = await _dbContext.FilmPosts
            .Where(p => p.UserId == user.Id)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ProfilePostViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                ImageUrl = p.ImageUrl
            })
            .ToListAsync();

        // Construct the view model
        return new ProfileViewModel
        {
            UserName = user.Username,
            UserID = user.Id,
            Email = user.Email,
            PostCount = posts.Count,
            FriendsCount = friendsCount,
            Posts = posts,
            friendshipState = friendshipState
        };
    }
}
