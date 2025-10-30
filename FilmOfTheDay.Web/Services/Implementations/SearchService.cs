using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Web.Models.Search;
using FilmOfTheDay.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FilmOfTheDay.Web.Services.Implementations;
public class SearchService : ISearchService
{
    private readonly ApplicationDbContext _dbContext;

    public SearchService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SearchViewModel> SearchUsersAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new SearchViewModel
            {
                Users = new List<SearchedUserViewModel>()
            };
        }

        var normalizedQuery = query.ToLower();

        var searchedUsers = await _dbContext.Users
            .AsNoTracking()
            .Where(u =>
                u.Username.ToLower().Contains(normalizedQuery) ||
                u.Email.ToLower().Contains(normalizedQuery))
            .Select(u => new SearchedUserViewModel
            {
                UserID = u.Id,
                UserName = u.Username,
                ProfileImageUrl = "/images/profile-placeholder.png",
                Email = u.Email
            })
            .ToListAsync();


        return new SearchViewModel
        {
            Users = searchedUsers
        };
    }
}
