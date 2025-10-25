using FilmOfTheDay.Web.Models.Home;

namespace FilmOfTheDay.Web.Services.Interfaces;
public interface IHomeFeedService
{
    public Task<HomeFeedViewModel> GetHomeFeedAsync(int userId);
}