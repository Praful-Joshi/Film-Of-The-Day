using FilmOfTheDay.Web.Models.Profile;
using Microsoft.AspNetCore.Mvc;

namespace FilmOfTheDay.Web.ViewComponents;
public class FriendshipButtonViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(ProfileViewModel model)
    {
        return View("Default", model);
    }
}
