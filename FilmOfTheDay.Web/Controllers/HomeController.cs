using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FilmOfTheDay.Web.Models;
using FilmOfTheDay.Web.Services.Interfaces;
using System.Security.Claims;

namespace FilmOfTheDay.Web.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHomeFeedService _homeFeedService;

    public HomeController(ILogger<HomeController> logger, IHomeFeedService homeFeedService)
    {
        _logger = logger;
        _homeFeedService = homeFeedService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        try
        {
            var feed = await _homeFeedService.GetHomeFeedAsync(userId);
            return View(feed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading home feed for user {UserId}", userId);
            TempData["Error"] = "Unable to load your feed right now. Please try again later.";
            return View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }

    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpGet]
    public IActionResult Error()
    {
        return View(new ErrorViewModel 
        { 
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
        });
    }

    // --- Private helper ---
    private bool TryGetUserId(out int userId)
    {
        userId = 0;
        var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(idValue, out userId);
    }
}
