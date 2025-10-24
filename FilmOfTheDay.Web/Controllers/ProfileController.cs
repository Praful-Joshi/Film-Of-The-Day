using FilmOfTheDay.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmOfTheDay.Web.Controllers;
[Authorize]
public class ProfileController : Controller
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int id)
    {
        var viewModel = await _profileService.GetProfileAsync(id, User);
        if (viewModel == null)
            return NotFound();

        return View(viewModel);
    }
}
