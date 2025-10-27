using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FilmOfTheDay.Web.Services.Interfaces;

namespace FilmOfTheDay.Web.ViewComponents;
public class NotificationBellViewComponent : ViewComponent
{
    private readonly IProfileService _profileService;

    public NotificationBellViewComponent(IProfileService profileService)
    {
        _profileService = profileService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (!User.Identity.IsAuthenticated)
            return View(false);

        var user = await _profileService.GetLoggedInUserAsync(UserClaimsPrincipal);
        if(user == null)
            return View(false);
        bool hasUnread = !user.ReadNotifications;
        return View(hasUnread);
    }
}
