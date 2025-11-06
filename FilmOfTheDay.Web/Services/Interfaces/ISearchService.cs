using System.Security.Claims;
using FilmOfTheDay.Web.Models.Search;

namespace FilmOfTheDay.Web.Services.Interfaces;
public interface ISearchService
{
    public Task<SearchViewModel> SearchUsersAsync(string query, ClaimsPrincipal loggedInUser);
}
