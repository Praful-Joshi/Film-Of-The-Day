using FilmOfTheDay.Web.Models.Search;

namespace FilmOfTheDay.Web.Services.Interfaces;
public interface ISearchService
{
    Task<SearchViewModel> SearchUsersAsync(string query);
}
