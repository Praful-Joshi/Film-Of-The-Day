using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Web.Models.Search;
using FilmOfTheDay.Web.Models.Profile;
using FilmOfTheDay.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FilmOfTheDay.Web.Services.Implementations;
public class SearchService : ISearchService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IConnectionService _connectionService;

    public SearchService(ApplicationDbContext dbContext, IConnectionService connectionService)
    {
        _dbContext = dbContext;
        _connectionService = connectionService;
    }

    public async Task<SearchViewModel> SearchUsersAsync(string query, ClaimsPrincipal loggedInUser)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new SearchViewModel
            {
                Users = null
            };
        }

        var normalizedQuery = query.ToLower();
        var loggedInUserIdStr = loggedInUser.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(loggedInUserIdStr) || !int.TryParse(loggedInUserIdStr, out var loggedInUserId))
        {
            return new SearchViewModel
            {
                Users = null
            };
        }

        // Step 1: Fetch matched users (basic info)
        var searchedUsers = await _dbContext.Users
            .AsNoTracking()
            .Where(u =>
                u.Username.ToLower().Contains(normalizedQuery))
            .Select(u => new ProfileViewModel
            {
                UserName = u.Username,
                UserID = u.Id,
                Email = u.Email
            })
            .ToListAsync();

        // Step 2: For each user, determine friendship state using ConnectionService
        foreach (var user in searchedUsers)
        {
            user.friendshipState = _connectionService.GetFriendshipState(loggedInUserId, user.UserID);
        }
        return new SearchViewModel
        {
            Users = searchedUsers
        };
    }
}
